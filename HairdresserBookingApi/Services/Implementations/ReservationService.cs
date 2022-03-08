using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HairdresserBookingApi.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    private readonly IAvailabilityService _availabilityService;

    public ReservationService(BookingDbContext dbContext, IMapper mapper, IUserContextService userContextService, IAvailabilityService availabilityService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userContextService = userContextService;
        _availabilityService = availabilityService;
    }


    private bool IsAccessible(ReservationDto request)
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

        var accessibilityList = new List<TimeRange>();

        if (list.Count > 0)
        {
            accessibilityList.Add(new TimeRange(workerAvailability.Start, list[0].From));

            for (int i = 0; i < reservations.Count - 1; i++)
            {
                accessibilityList.Add(new TimeRange(list[i].To, list[i + 1].From));
            }

            accessibilityList.Add(new TimeRange(list[reservations.Count - 1].To, workerAvailability.End));
        }
        else
        {
            accessibilityList.Add(new TimeRange(workerAvailability.Start, workerAvailability.End));
        }



        return accessibilityList;
    }

    public List<TimeRange> GetAllPossibleTimesInDay(ReservationDto request)
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
}