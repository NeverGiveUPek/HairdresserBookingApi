using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;




[ApiController]
[Route("api/reservation")]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    
    [HttpGet("day")]
    public ActionResult GetAllPossibleTimesInDay([FromQuery] ReservationRequestDto reservationRequest)
    {
        var allPossibleTimes = _reservationService.GetAllPossibleTimesInDay(reservationRequest);

        return Ok(allPossibleTimes);
    }


    [HttpPost]
    public ActionResult MakeReservation([FromBody] ReservationRequestDto reservation)
    {
        _reservationService.MakeReservation(reservation);

        return Created("", null);
    }

    [HttpGet("all")]
    public ActionResult<IEnumerable<ReservationInfoDto>> GetAllReservations()
    {
        var reservationsInfo = _reservationService.GetAllReservations();

        return Ok(reservationsInfo);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ReservationInfoDto>> GetFutureReservations()
    {
        var reservationsInfo = _reservationService.GetFutureReservations();

        return Ok(reservationsInfo);
    }





}