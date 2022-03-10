using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.WorkerActivity;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HairdresserBookingApi.Services.Implementations;

public class WorkerActivityService : IWorkerActivityService
{

    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public WorkerActivityService(BookingDbContext dbContext, IMapper mapper, IUserContextService userContextService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userContextService = userContextService;
    }


    public List<WorkerActivityDto> GetAllActivitiesOfWorker(int workerId)
    {
        var worker = _dbContext
            .Workers
            .Include(w => w.WorkerActivity)
            .SingleOrDefault(w => w.Id == workerId);

        if (worker == null) throw new NotFoundException($"Worker of Id: {workerId} is not found");

        var workerActivities = worker.WorkerActivity;

        var workerActivitiesDto = _mapper.Map<List<WorkerActivityDto>>(workerActivities);

        return workerActivitiesDto;
    }

    public List<WorkerActivityDto> GetAllWorkersOfActivity(int activityId)
    {
        var activity = _dbContext
            .Activities
            .Include(a => a.WorkerActivity)
            .SingleOrDefault(a => a.Id == activityId);

        if (activity == null) throw new NotFoundException($"Activity of Id: {activityId} is not found");

        var workerActivities = activity.WorkerActivity;

        var workerActivitiesDto = _mapper.Map<List<WorkerActivityDto>>(workerActivities);

        return workerActivitiesDto;
    }
}