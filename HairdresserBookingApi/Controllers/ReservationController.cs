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

    
    [HttpGet("day")]
    public IActionResult GetAllPossibleTimesInDay([FromQuery] ReservationDto reservationRequest)
    {
        var allPossibleTimes = _reservationService.GetAllPossibleTimesInDay(reservationRequest);

        return Ok(allPossibleTimes);
    }



}