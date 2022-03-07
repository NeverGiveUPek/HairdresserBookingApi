using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;




[ApiController]
[Route("api/reservation")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }


    [HttpPost]
    public IActionResult Test([FromBody] ReservationRequestDto request)
    {
        var isAccessible = _reservationService.IsAccessible(request);

        return Ok();
    }


}