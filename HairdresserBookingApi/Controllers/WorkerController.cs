using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Worker;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;

[ApiController]
[Route("api/worker")]
[Authorize(Roles = "Admin,Manager")]
public class WorkerController : ControllerBase
{
    private readonly IWorkerService _workerService;

    public WorkerController(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    
    [HttpGet]
    [Authorize]
    public ActionResult<List<WorkerDto>> GetAll()
    {
        var workersDto = _workerService.GetAll();

        return Ok(workersDto);
    }

    [HttpGet("{id}")]
    public ActionResult<WorkerDetailsDto> GetById([FromRoute] int id)
    {
        var workerDetailsDto = _workerService.GetById(id);

        return Ok(workerDetailsDto);
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateWorkerDto dto)
    {
        var workerId = _workerService.Create(dto);

        return Created($"api/worker/{workerId}", null);
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromBody] UpdateWorkerDto dto, int id)
    {
        _workerService.Update(dto, id);

        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _workerService.Delete(id);

        return NoContent();
    }

}