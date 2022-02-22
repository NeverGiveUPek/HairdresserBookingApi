using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Activity;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HairdresserBookingApi.Services.Implementations;


public class ActivityService : IActivityService
{

    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;

    public ActivityService(BookingDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public List<ActivityDto> GetAll()
    {
        var services = _dbContext
            .Activities
            .ToList();

        var servicesDto = _mapper.Map<List<ActivityDto>>(services);

        return servicesDto;
    }

    public List<AvailableActivityDto> GetAllAvailable()
    {
        var availableServices = _dbContext
            .WorkerActivities
            .Include(ws => ws.Activity)
            .ToList();

        var availableCheapestServices = availableServices
            .OrderBy(ws => ws.Price)
            .GroupBy(ws => ws.ActivityId)
            .Select(wsGroup => wsGroup.First())
            .ToList();
            

        var availableServicesDto = _mapper.Map<List<AvailableActivityDto>>(availableCheapestServices);

        return availableServicesDto;
    }

    public void Create(CreateActivityDto dto)
    {
        throw new NotImplementedException();
    }
}