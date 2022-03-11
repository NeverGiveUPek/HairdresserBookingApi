using AutoMapper;
using HairdresserBookingApi.Authorization;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HairdresserBookingApi.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IAvailabilityService _availabilityService;
    private readonly IAuthorizationService _authorizationService;

    public ReservationService(BookingDbContext dbContext, IMapper mapper, IUserContextService userContextService, IAvailabilityService availabilityService, IAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userContextService = userContextService;
        _availabilityService = availabilityService;
        _authorizationService = authorizationService;
    }



    public List<TimeRange> GetAllPossibleTimesInDay(ReservationRequestDto request)
    {

        var workerActivity = _dbContext
            .WorkerActivities
            .Include(wa => wa.Worker)
            .ThenInclude(w => w.Availabilities)
            .FirstOrDefault(wa => wa.Id == request.WorkerActivityId);

        if (workerActivity == null)
            throw new NotFoundException($"Worker activity of id {request.WorkerActivityId} is not found in database");


        var accessibilityList = AccessibilityInDay(request.Date.Date, workerActivity.WorkerId);



        var possibleTimes = new List<TimeRange>();

        foreach (var timeRange in accessibilityList)
        {
            if(timeRange.From <= timeRange.To.AddMinutes(-1 * workerActivity.RequiredMinutes))
            {
                possibleTimes.Add(new TimeRange(timeRange.From, timeRange.To.AddMinutes(-1 * workerActivity.RequiredMinutes)));
            }
        }

        return possibleTimes;

    }

    public void MakeReservation(ReservationRequestDto request)
    {
        

        var isAccessible = IsAccessible(request);

        if (!isAccessible) throw new NotAccessibleException($"Reservation can't be in this time");
        
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

        var reservations = _dbContext.Reservations.Include(r => r.WorkerActivity).Where(r => r.UserId == userId && r.Date > DateTime.Now);

        var reservationsInfo = _mapper.Map<List<ReservationInfoDto>>(reservations);

        return reservationsInfo;
    }

    public void DeleteReservation(int reservationId)
    {
        var reservation = _dbContext
            .Reservations
            .FirstOrDefault(r => r.Id == reservationId);

        if (reservation == null) throw new NotFoundException($"Reservation of id: {reservationId} is not found");

        var authorizeResult = _authorizationService.AuthorizeAsync(_userContextService.GetUser()!, reservation,
            new OperationRequirement(Operation.Delete)).Result;

        if (!authorizeResult.Succeeded)
        {
            throw new ForbidException($"Don't have rights to change this resource");
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

        var authorizeResult = _authorizationService.AuthorizeAsync(_userContextService.GetUser()!, reservation,
            new OperationRequirement(Operation.Update)).Result;

        if (!authorizeResult.Succeeded)
        {
            throw new ForbidException($"Don't have rights to change this resource");
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

    private bool IsAccessible(ReservationRequestDto request)
    {
        var possibleTimesInDay = GetAllPossibleTimesInDay(request);

        foreach (var timeRange in possibleTimesInDay)
        {
            if (request.Date >= timeRange.From && request.Date <= timeRange.To)
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

            list.Add(new TimeRange(start,end));   
        }

        //collapse reservationRequest times example: 11:00-11:30 and 11:30-12:00 => 11:00-12:00

        for (int i = list.Count - 1; i > 0 ; i--)
        {
            if (list[i].From == list[i - 1].To)
            {
                list[i-1].To = list[i].To;
                list.RemoveAt(i);
            }
        }





        var accessibilityList = new List<TimeRange>();

        if (list.Count > 0)
        {
            accessibilityList.Add(new TimeRange(workerAvailability.Start, list[0].From));

            for (int i = 0; i < list.Count - 1; i++)
            {
                accessibilityList.Add(new TimeRange(list[i].To, list[i + 1].From));
            }

            accessibilityList.Add(new TimeRange(list[^1].To, workerAvailability.End)); //[^1] - first from the end 
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