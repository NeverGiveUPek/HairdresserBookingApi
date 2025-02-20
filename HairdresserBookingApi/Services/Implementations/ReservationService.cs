﻿using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using HairdresserBookingApi.Services.Strategies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NLog.Web.LayoutRenderers;

namespace HairdresserBookingApi.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IAvailabilityService _availabilityService;
    private readonly IAuthorizationService _authorizationService;

    private readonly IReservationSelectorStrategy _reservationSelector;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(BookingDbContext dbContext, IMapper mapper, IUserContextService userContextService,
        IAvailabilityService availabilityService, IAuthorizationService authorizationService,
        ILogger<ReservationService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userContextService = userContextService;
        _availabilityService = availabilityService;
        _authorizationService = authorizationService;
        _logger = logger;

        _reservationSelector = new FastestReservationSelectorStrategy();
    }


    /// <summary>
    /// Function gets all time in day where reservation can fit (it ends before or equal time where worker is accessible)
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="AppException"></exception>
    public List<TimeRange> GetAllPossibleTimesInDay(ReservationRequestDto request)
    {
        var workerActivity = _dbContext
            .WorkerActivities
            .Include(wa => wa.Worker)
            .ThenInclude(w => w.Availabilities)
            .FirstOrDefault(wa => wa.Id == request.WorkerActivityId);

        if (workerActivity == null)
            throw new NotFoundException($"Worker activity of id {request.WorkerActivityId} is not found in database");

        if (!workerActivity.IsActive) throw new AppException($"WorkerActivity is not active");


        var accessibilityList = AccessibilityInDay(request.Date.Date, workerActivity.WorkerId);


        var possibleTimes = new List<TimeRange>();

        foreach (var timeRange in accessibilityList)
        {
            if (timeRange.StartDate <= timeRange.EndDate.AddMinutes(-1 * workerActivity.RequiredMinutes))
            {
                possibleTimes.Add(new TimeRange(timeRange.StartDate,
                    timeRange.EndDate.AddMinutes(-1 * workerActivity.RequiredMinutes)));
            }
        }

        return possibleTimes;
    }

    public void MakeReservation(ReservationRequestDto request)
    {
        var isAccessible = IsAccessible(request);

        if (!isAccessible) throw new NotAccessibleException($"Reservation can't be in this time");

        var workerActivity = _dbContext.WorkerActivities.SingleOrDefault(x => x.Id == request.WorkerActivityId);

        if (workerActivity == null)
            throw new NotFoundException($"WorkerActivity of Id: {request.WorkerActivityId} is not found");

        if (!workerActivity.IsActive) throw new AppException($"WorkerActivity is not active");


        var userId = _userContextService.GetUserId();
        if (userId == null) throw new AppException($"Can't receive userId from Claims");

        var amountOfFutureReservation = CountAmountOfUserFutureReservation((int) userId);

        if (amountOfFutureReservation >= 5) throw new ForbidException($"Maximum amount of current reservation is 5");

        var reservationEntity = _mapper.Map<Reservation>(request);


        reservationEntity.UserId = (int) userId;

        _dbContext.Reservations.Add(reservationEntity);
        _dbContext.SaveChanges();
    }

    public List<ReservationInfoDto> GetAllUserReservations()
    {
        var userId = _userContextService.GetUserId();
        if (userId == null) throw new AppException($"Can't receive userId from Claims");

        var reservations = _dbContext.Reservations.Include(r => r.WorkerActivity).Where(r => r.UserId == userId);

        var reservationsInfo = _mapper.Map<List<ReservationInfoDto>>(reservations);

        return reservationsInfo;
    }

    public List<ReservationInfoDto> GetFutureUserReservations()
    {
        var userId = _userContextService.GetUserId();
        if (userId == null) throw new AppException($"Can't receive userId from Claims");

        var reservations = _dbContext.Reservations.Include(r => r.WorkerActivity)
            .Where(r => r.UserId == userId && r.Date > DateTime.Now);

        var reservationsInfo = _mapper.Map<List<ReservationInfoDto>>(reservations);

        return reservationsInfo;
    }

    public void DeleteReservation(int reservationId)
    {
        _logger.LogWarning($"DELETE action for reservation of id: {reservationId}");

        var reservation = _dbContext
            .Reservations
            .FirstOrDefault(r => r.Id == reservationId);

        if (reservation == null) throw new NotFoundException($"Reservation of id: {reservationId} is not found");

        var role = _userContextService.GetUserRole();

        if (role == null) throw new ForbidException($"Don't have rights to change this resource");

        var id = _userContextService.GetUserId();

        if (role == null || (role != "Admin" && role != "Manager" && id != reservation.UserId))
        {
            throw new ForbidException($"You don't have rights to change this resource");
        }


        _dbContext.Reservations.Remove(reservation);
        _dbContext.SaveChanges();
    }

    public void EditReservation(int reservationId, EditReservationDateDto editReservationDto)
    {
        var reservation = _dbContext
            .Reservations
            .FirstOrDefault(r => r.Id == reservationId);

        if (reservation == null) throw new NotFoundException($"Reservation of id: {reservationId} is not found");


        var id = _userContextService.GetUserId();

        if (id == null || id != reservation.UserId)
        {
            throw new ForbidException($"You don't have rights to change this resource");
        }

        //check if new date is possible
        var isPossible = IsAccessible(new ReservationRequestDto
        {
            Date = editReservationDto.Date,
            WorkerActivityId = reservation.WorkerActivityId
        });

        if (!isPossible) throw new NotAccessibleException($"This time is not accessible");

        reservation.Date = editReservationDto.Date;

        _dbContext.SaveChanges();
    }

    private IReservationSelectorStrategy SelectStrategy(PickStrategy pickStrategy)
    {
        IReservationSelectorStrategy strategy;

        switch (pickStrategy)
        {
            case PickStrategy.Fast:
                strategy = new FastestReservationSelectorStrategy();
                break;
            case PickStrategy.Early:
                strategy = new EarliestReservationSelectorStrategy();
                break;
            case PickStrategy.MostAccessible:
                strategy = new MostAccessibleTimeReservationSelectorStrategy();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return strategy;
    }

    public ReservationRequestDto FindBestReservation(ReservationRequirementDto requirement)
    {
        IReservationSelectorStrategy strategy = SelectStrategy(requirement.PickStrategy);


        var accessibility = new List<TimeRange>();


        while (requirement.TimeRange.StartDate.Date <= requirement.TimeRange.EndDate.Date)
        {
            accessibility.AddRange(GetAllPossibleTimesInDay(new ReservationRequestDto()
            {
                Date = requirement.TimeRange.StartDate,
                WorkerActivityId = requirement.WorkerActivityId
            }));

            requirement.TimeRange.StartDate = requirement.TimeRange.StartDate.AddDays(1);
        }


        var bestTime = strategy.FindBestTime(accessibility);

        if (bestTime is null) throw new NotAccessibleException($"There aren't any possible times to make reservation");

        var reservationRequest = new ReservationRequestDto()
        {
            Date = bestTime.Value,
            WorkerActivityId = requirement.WorkerActivityId
        };

        return reservationRequest;
    }

    private bool IsAccessible(ReservationRequestDto request)
    {
        var possibleTimesInDay = GetAllPossibleTimesInDay(request);

        foreach (var timeRange in possibleTimesInDay)
        {
            if (request.Date >= timeRange.StartDate && request.Date <= timeRange.EndDate)
            {
                return true;
            }
        }


        return false;
    }


    private List<TimeRange> AccessibilityInDay(DateTime date, int workerId)
    {
        var workerAvailability = _availabilityService.AvailabilityInDay(date, workerId);

        if (workerAvailability == null)
        {
            return new List<TimeRange>();
        }

        var reservations = _dbContext
            .Reservations
            .Include(r => r.WorkerActivity)
            .Where(r => r.Date.Date == date.Date && r.WorkerActivity.WorkerId == workerId)
            .OrderBy(r => r.Date)
            .ToList();


        var list = new List<TimeRange>();

        foreach (var reservation in reservations)
        {
            var start = reservation.Date;
            var end = reservation.Date.AddMinutes(reservation.WorkerActivity.RequiredMinutes);

            list.Add(new TimeRange(start, end));
        }

        //collapse reservationRequest times example: 11:00-11:30 and 11:30-12:00 => 11:00-12:00

        for (int i = list.Count - 1; i > 0; i--)
        {
            if (list[i].StartDate == list[i - 1].EndDate)
            {
                list[i - 1].EndDate = list[i].EndDate;
                list.RemoveAt(i);
            }
        }


        var accessibilityList = new List<TimeRange>();

        if (list.Count > 0)
        {
            accessibilityList.Add(new TimeRange(workerAvailability.Start, list[0].StartDate));

            for (int i = 0; i < list.Count - 1; i++)
            {
                accessibilityList.Add(new TimeRange(list[i].EndDate, list[i + 1].StartDate));
            }

            accessibilityList.Add(new TimeRange(list[^1].EndDate, workerAvailability.End)); //[^1] - first from the end 
        }
        else
        {
            accessibilityList.Add(new TimeRange(workerAvailability.Start, workerAvailability.End));
        }


        return accessibilityList;
    }

    private int CountAmountOfUserFutureReservation(int userId)
    {
        var amount = _dbContext.Reservations.Count(r => r.UserId == userId && r.Date > DateTime.Now);

        return amount;
    }
}