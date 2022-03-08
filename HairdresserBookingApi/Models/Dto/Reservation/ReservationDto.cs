using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Dto.Reservation;

public class ReservationDto
{

    [Required] 
    public DateTime Date { get; set; }

    [Required]
    public int WorkerActivityId { get; set; }

}