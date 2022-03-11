﻿using HairdresserBookingApi.Models.Dto.WorkerActivity;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IWorkerActivityService
{ 
    List<WorkerActivityDto> GetAllActivitiesOfWorker(int workerId);

    List<WorkerActivityDto> GetAllWorkersOfActivity(int activityId);

    WorkerActivityDto GetWorkerActivity(int id);
    int CreateWorkerActivity(CreateWorkerActivityDto dto);
}