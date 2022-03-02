using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;


[ApiController]
[Route("api/worker/{workerId}/availability")]
public class AvailabilityController : ControllerBase
{
    private readonly IAvailabilityService _availabilityService;

    public AvailabilityController(IAvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }


    [HttpGet("current")]
    public ActionResult GetCurrentAvailabilities([FromRoute] int workerId)
    {
        var currentAvailabilities = _availabilityService.GetCurrentAvailabilities(workerId);

        return Ok(currentAvailabilities);
    }

    [HttpGet("all")]
    public ActionResult GetAllAvailabilities([FromRoute] int workerId)
    {
        var currentAvailabilities = _availabilityService.GetAllAvailabilities(workerId);

        return Ok(currentAvailabilities);
    }

    [HttpPost]
    public ActionResult AddAvailability([FromBody] AddAvailabilityDto dto, [FromRoute] int workerId)
    {
        _availabilityService.AddAvailability(dto, workerId);

        return Created($"api/worker/{workerId}/availability/current", null);
    }


}