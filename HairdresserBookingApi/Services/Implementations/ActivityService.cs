using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Activity;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HairdresserBookingApi.Services.Implementations;


public class ActivityService : IActivityService
{

    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<ActivityService> _logger;

    public ActivityService(BookingDbContext dbContext, IMapper mapper, ILogger<ActivityService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
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
            .Where(wa => wa.IsActive == true)
            .ToList();

        var availableCheapestServices = availableServices
            .OrderBy(ws => ws.Price)
            .GroupBy(ws => ws.ActivityId)
            .Select(wsGroup => wsGroup.First())
            .ToList();
            

        var availableServicesDto = _mapper.Map<List<AvailableActivityDto>>(availableCheapestServices);

        return availableServicesDto;
    }

    public ActivityDetailsDto GetById(int id)
    {
        var activity = _dbContext
            .Activities
            .Include(a => a.WorkerActivity)
            .ThenInclude(wa => wa.Worker)
            .SingleOrDefault(a => a.Id == id);

        if (activity == null) throw new NotFoundException($"Activity of id: {id} is not found");

        
        activity.WorkerActivity = activity.WorkerActivity.Where(x => x.IsActive).ToList();

        var activityDetailsDto = _mapper.Map<ActivityDetailsDto>(activity);

        return activityDetailsDto;
    }



    public int Create(CreateActivityDto dto)
    {
        var foundActivity = _dbContext
            .Activities
            .SingleOrDefault(a => a.Name == dto.Name && a.IsForMan == dto.IsForMan);

        if (foundActivity != null)
            throw new EntityExistsException($"Entity with name:{dto.Name} and value of IsForMan:{dto.IsForMan} is already in database");

        var activity = _mapper.Map<Activity>(dto);


        _dbContext.Activities.Add(activity);
        _dbContext.SaveChanges();

        return activity.Id;
    }

    public void Delete(int id)
    {
        _logger.LogWarning($"DELETE action for Activity of id: {id}");

        var activity = _dbContext
            .Activities
            .Include(a => a.WorkerActivity)
            .ThenInclude(wa => wa.Reservations)
            .SingleOrDefault(a => a.Id == id);

        if (activity == null) throw new NotFoundException($"Activity of id: {id} is not found");


        _dbContext.Activities.Remove(activity);
        _dbContext.SaveChanges();
    }

    public void Update(UpdateActivityDto dto, int id)
    {
        var foundActivity = _dbContext
            .Activities
            .SingleOrDefault(a => a.Id == id);

        if (foundActivity == null) throw new NotFoundException($"Activity of id: {id} is not found");

        foundActivity.Name = dto.Name;
        foundActivity.Description = dto.Description;
        foundActivity.IsForMan = dto.IsForMan;


        _dbContext.SaveChanges();
    }
}