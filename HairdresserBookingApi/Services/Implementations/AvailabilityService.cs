using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HairdresserBookingApi.Services.Implementations;

public class AvailabilityService : IAvailabilityService
{
    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;

    public AvailabilityService(BookingDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public List<AvailabilityDto> GetAllAvailabilities(int workerId)
    {
        var worker = _dbContext
            .Workers
            .Include(w => w.Availabilities)
            .SingleOrDefault(w => w.Id == workerId);

        if (worker == null) throw new NotFoundException($"Worker of id {workerId} is not found");

        var availabilities = worker.Availabilities.ToList();

        var availabilitiesDto = _mapper.Map<List<AvailabilityDto>>(availabilities);

        return availabilitiesDto;
    }

    public List<AvailabilityDto> GetCurrentAvailabilities(int workerId)
    {
        var worker = _dbContext
            .Workers
            .Include(w => w.Availabilities)
            .SingleOrDefault(w => w.Id == workerId);

        if (worker == null) throw new NotFoundException($"Worker of id {workerId} is not found");

        var availabilities = worker.Availabilities.Where(a => a.Start.Day >= DateTime.Now.Day).ToList();

        var availabilitiesDto = _mapper.Map<List<AvailabilityDto>>(availabilities);

        return availabilitiesDto;
    }

    public void AddAvailability(AddAvailabilityDto dto, int workerId)
    {
        var worker = _dbContext
            .Workers
            .Include(w => w.Availabilities)
            .SingleOrDefault(w => w.Id == workerId);

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
}