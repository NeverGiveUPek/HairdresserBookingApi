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

    [HttpGet("{id}")]
    public ActionResult<WorkerActivityDto> GetWorkerActivity([FromRoute] int id)
    {
        var result = _workerActivityService.GetWorkerActivity(id);

        return Ok(result);
    }


    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult CreateWorkerActivity(CreateWorkerActivityDto dto)
    {
        var id = _workerActivityService.CreateWorkerActivity(dto);

        return Created($"api/workerActivity/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteWorkerActivity([FromRoute] int id)
    {
        _workerActivityService.Delete(id);

        return NoContent();
    }

    [HttpPut("{id}/deactivate")]
    public ActionResult DeactivateWorkerActivity([FromRoute] int id)
    {
        _workerActivityService.Deactivate(id);

        return NoContent();
    }

    [HttpPut("{id}/activate")]
    public ActionResult ActivateWorkerActivity([FromRoute] int id)
    {
        _workerActivityService.Activate(id);

        return NoContent();
    }

}