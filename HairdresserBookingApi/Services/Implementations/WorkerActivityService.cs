using System.Xml;
using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.WorkerActivity;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HairdresserBookingApi.Services.Implementations;

public class WorkerActivityService : IWorkerActivityService
{

    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<WorkerActivityService> _logger;


    public WorkerActivityService(BookingDbContext dbContext, IMapper mapper, ILogger<WorkerActivityService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }


    public List<WorkerActivityDto> GetAllActivitiesOfWorker(int workerId)
    {
        var worker = _dbContext
            .Workers
            .Include(w => w.WorkerActivity)
            .ThenInclude(wa => wa.Activity)
            .SingleOrDefault(w => w.Id == workerId);

        if (worker == null) throw new NotFoundException($"Worker of Id: {workerId} is not found");

        var workerActivities = worker.WorkerActivity.Where(x => x.IsActive);

        var workerActivitiesDto = _mapper.Map<List<WorkerActivityDto>>(workerActivities);

        return workerActivitiesDto;
    }

    public List<WorkerActivityDto> GetAllWorkersOfActivity(int activityId)
    {
        var activity = _dbContext
            .Activities
            .Include(a => a.WorkerActivity)
            .ThenInclude(wa => wa.Worker)
            .SingleOrDefault(a => a.Id == activityId);

        if (activity == null) throw new NotFoundException($"Activity of Id: {activityId} is not found");

        var workerActivities = activity.WorkerActivity.Where(x => x.IsActive);

        var workerActivitiesDto = _mapper.Map<List<WorkerActivityDto>>(workerActivities);

        return workerActivitiesDto;
    }


    public WorkerActivityDto GetWorkerActivity(int id)
    {
        var workerActivity = _dbContext.WorkerActivities.SingleOrDefault(wa => wa.Id == id && wa.IsActive);

        if (workerActivity == null) throw new NotFoundException($"WorkerActivity of Id: {id} is not found");

        var workerActivityDto = _mapper.Map<WorkerActivityDto>(workerActivity);

        return workerActivityDto;
    }

    public int CreateWorkerActivity(CreateWorkerActivityDto dto)
    {
        var activity = _dbContext.Activities.SingleOrDefault(a => a.Id == dto.ActivityId);
        if (activity == null) throw new NotFoundException($"Activity of Id: {dto.ActivityId} is not found");

        var worker = _dbContext.Workers.SingleOrDefault(w => w.Id == dto.WorkerId);
        if (worker == null) throw new NotFoundException($"Worker of Id: {dto.WorkerId} is not found");


        var workerActivity = _mapper.Map<WorkerActivity>(dto);

        //check if same entity exist
        var foundEntity = _dbContext.WorkerActivities.FirstOrDefault
        (wa => wa.Price == dto.Price && wa.RequiredMinutes == dto.RequiredMinutes && wa.ActivityId == dto.ActivityId && wa.WorkerId == dto.WorkerId);

        if (foundEntity != null) throw new EntityExistsException($"Entity of this values already exists");


        _dbContext.WorkerActivities.Add(workerActivity);
        _dbContext.SaveChanges();

        return workerActivity.Id;
    }

    public void Delete(int id)
    {
        _logger.LogWarning($"DELETE action for workerActivity of id: {id}");

        var workerActivity = _dbContext
            .WorkerActivities
            .SingleOrDefault(x => x.Id == id);

        if (workerActivity == null) throw new NotFoundException($"WorkerActivity of Id: {id} is not found");

        var futureReservations = _dbContext.Reservations.Where(r => r.WorkerActivityId == id);

        if (futureReservations.Any())
            throw new AppException(
                $"Can't delete WorkerActivity with future reservation, disable WorkerActivity IsActive and wait until reservations execute");

        _dbContext.WorkerActivities.Remove(workerActivity);
        _dbContext.SaveChanges();
    }

    public void Deactivate(int id)
    {
        var workerActivity = _dbContext
            .WorkerActivities
            .SingleOrDefault(x => x.Id == id);

        if (workerActivity == null) throw new NotFoundException($"WorkerActivity of Id: {id} is not found");

        workerActivity.IsActive = false;

        _dbContext.SaveChanges();
    }

    public void Activate(int id)
    {
        var workerActivity = _dbContext
            .WorkerActivities
            .SingleOrDefault(x => x.Id == id);

        if (workerActivity == null) throw new NotFoundException($"WorkerActivity of Id: {id} is not found");

        workerActivity.IsActive = true;

        _dbContext.SaveChanges();
    }
}