using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;


[ApiController]
[Route("api/worker/{workerId}/availability")]
public class AvailabilityController : ControllerBase
{
    [HttpGet("current")]
    public ActionResult GetCurrentAvailability()
    {


        return Ok();
    }


}