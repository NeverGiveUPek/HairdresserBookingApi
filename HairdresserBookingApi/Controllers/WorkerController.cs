using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Worker;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;

[ApiController]
[Route("api/worker")]
public class WorkerController : ControllerBase
{
    private readonly IWorkerService _workerService;

    public WorkerController(IWorkerService workerService)
    {
        _workerService = workerService;
    }

    [HttpGet]
    public ActionResult<List<WorkerDto>> GetAll()
    {
        var workersDto = _workerService.GetAll();

        return workersDto;
    }

    [HttpGet("{id}")]
    public ActionResult<WorkerDetailsDto> GetById([FromRoute] int id)
    {
        var workerDetailsDto = _workerService.GetById(id);

        return workerDetailsDto;
    }



}