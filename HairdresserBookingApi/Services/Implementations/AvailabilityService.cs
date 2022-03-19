using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TimeRange = HairdresserBookingApi.Models.Dto.Helper.TimeRange;

namespace HairdresserBookingApi.Services.Implementations;

public class AvailabilityService : IAvailabilityService
{
    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<AvailabilityService> _logger;

    public AvailabilityService(BookingDbContext dbContext, IMapper mapper, ILogger<AvailabilityService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }


    public List<AvailabilityDto> GetAllAvailabilities(int workerId)
    {
        var worker = GetWorker(workerId);

        if (worker == null) throw new NotFoundException($"Worker of id {workerId} is not found");

        var availabilities = worker.Availabilities.ToList();

        var availabilitiesDto = _mapper.Map<List<AvailabilityDto>>(availabilities);

        return availabilitiesDto;
    }

    public List<AvailabilityDto> GetCurrentAvailabilities(int workerId)
    {
        var worker = GetWorker(workerId);

        if (worker == null) throw new NotFoundException($"Worker of id {workerId} is not found");

        var availabilities = worker.Availabilities.Where(a => a.Start.Day >= DateTime.Now.Day).ToList();

        var availabilitiesDto = _mapper.Map<List<AvailabilityDto>>(availabilities);

        return availabilitiesDto;
    }

    public void AddAvailability(AddAvailabilityDto dto, int workerId)
    {
        var worker = GetWorker(workerId);

        if (worker == null) throw new NotFoundException($"Worker of id {workerId} is not found");

        var sameTimeAvailability = worker.Availabilities.FirstOrDefault(a => a.WorkerId == workerId && a.Start.Day == dto.Start.Day);
        
        if (sameTimeAvailability != null)
        {
            sameTimeAvailability.Start = dto.Start;
            sameTimeAvailability.End = dto.End;
            return;
        }


        var availability = _mapper.Map<Availability>(dto);
        availability.WorkerId = workerId;

        _dbContext.Availabilities.Add(availability);
        _dbContext.SaveChanges();
    }

    public void Delete(int id, int workerId)
    {
        _logger.LogWarning($"DELETE action for availability of id: {id}");

        var worker = GetWorker(workerId);

        if (worker == null) throw new NotFoundException($"Worker of id {workerId} is not found");

        var availabilityToDelete = worker.Availabilities.SingleOrDefault(a => a.Id == id);

        if (availabilityToDelete == null) throw new NotFoundException($"Availability of this id is not found");

        _dbContext.Availabilities.Remove(availabilityToDelete);
        _dbContext.SaveChanges();
    }

    public void Update(int id, int workerId, UpdateAvailabilityDto dto)
    {
        var worker = GetWorker(workerId);

        if (worker == null) throw new NotFoundException($"Worker of id {workerId} is not found");

        var availabilityToChange = worker.Availabilities.SingleOrDefault(a => a.Id == id);

        if (availabilityToChange == null) throw new NotFoundException($"Availability of this id is not found");


        if (availabilityToChange.Start.Day != dto.Start.Day || availabilityToChange.End.Day != dto.End.Day)
            throw new InvalidOperationException($"You can't change days of availability");
        
        availabilityToChange.Start = dto.Start;
        availabilityToChange.End = dto.End;

        _dbContext.SaveChanges();

    }

    public AvailabilityDto? AvailabilityInDay(DateTime date, int workerId)
    {
        var worker = GetWorker(workerId);

        if (worker == null) throw new NotFoundException($"Worker of id {workerId} is not found");

        
        var availabilityInDay =  worker
            .Availabilities
            .FirstOrDefault(w => w.Start.Date == date.Date);


        var availabilityInDayDto = _mapper.Map<AvailabilityDto>(availabilityInDay);

        return availabilityInDayDto;
    }

    public void AddAvailabilityInPeriod(TimeRange timeRange, int workerId)
    {
        var worker = GetWorker(workerId);
        if (worker == null) throw new NotFoundException($"Worker of id {workerId} is not found");

        var availabilitiesToAdd = new List<Availability>();
        
        var workDuration = timeRange.EndDate.TimeOfDay - timeRange.StartDate.TimeOfDay;

        while (timeRange.StartDate.Date <= timeRange.EndDate.Date)
        {
            availabilitiesToAdd.Add(new Availability()
            {
                Start = timeRange.StartDate,
                End = timeRange.StartDate.Add(workDuration),
                WorkerId = workerId
            });
            timeRange.StartDate = timeRange.StartDate.AddDays(1);
        }


        foreach (var element in availabilitiesToAdd)
        {
            var foundAvailability = _dbContext.Availabilities.FirstOrDefault(a => a.Start.Day == element.Start.Day && a.WorkerId == element.WorkerId);
            if (foundAvailability == null)
            {
                _dbContext.Availabilities.Add(element);
            }
        }

        _dbContext.SaveChanges();


    }


    private Worker? GetWorker(int workerId)
    {
        var worker = _dbContext
            .Workers
            .Include(w => w.Availabilities)
            .SingleOrDefault(w => w.Id == workerId);

        return worker;
    }



}