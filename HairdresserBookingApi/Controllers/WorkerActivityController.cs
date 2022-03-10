using HairdresserBookingApi.Models.Dto.WorkerActivity;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;


[ApiController]
[Route("api/workerActivity")]
//[Authorize]
public class WorkerActivityController : ControllerBase
{
    private readonly IWorkerActivityService _workerActivityService;

    public WorkerActivityController(IWorkerActivityService workerActivityService)
    {
        _workerActivityService = workerActivityService;
    }


    [HttpGet("worker/{workerId}")]
    public ActionResult<IEnumerable<WorkerActivityDto>> GetAllActivitiesOfWorker([FromRoute] int workerId)
    {
        var result = _workerActivityService.GetAllActivitiesOfWorker(workerId);


        return Ok(result);
    }


    [HttpGet("activity/{activityId}")]
    public ActionResult<IEnumerable<WorkerActivityDto>> GetAllWorkersOfActivity([FromRoute] int activityId)
    {
        var result = _workerActivityService.GetAllWorkersOfActivity(activityId);

        return Ok(result);
    }


}