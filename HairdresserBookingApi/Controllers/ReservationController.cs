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
    public ActionResult<IEnumerable<ReservationInfoDto>> GetAllUserReservations()
    {
        var reservationsInfo = _reservationService.GetAllUserReservations();

        return Ok(reservationsInfo);
    }

    [HttpGet]
    public ActionResult<IEnumerable<ReservationInfoDto>> GetFutureUserReservations()
    {
        var reservationsInfo = _reservationService.GetFutureUserReservations();

        return Ok(reservationsInfo);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteReservation([FromRoute] int id)
    {
        _reservationService.DeleteReservation(id);

        return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult EditReservation([FromRoute] int id, [FromBody] EditReservationDateDto editReservationDateDto)
    {
        _reservationService.EditReservation(id,editReservationDateDto);

        return Ok();
    }





}