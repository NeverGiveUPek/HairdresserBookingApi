using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Dto.Reservation;

public class ReservationRequestDto
{
    //validation in ReservationRequestDtoValidator

    public DateTime Date { get; set; }
    public int WorkerActivityId { get; set; }

}