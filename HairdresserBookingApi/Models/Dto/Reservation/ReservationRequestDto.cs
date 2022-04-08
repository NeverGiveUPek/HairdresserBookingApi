using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HairdresserBookingApi.Models.Dto.Reservation;

public class ReservationRequestDto
{
    //validation in ReservationRequestDtoValidator

    public DateTime Date { get; set; }
    public int WorkerActivityId { get; set; }


    public override string ToString()
    {
        var date = Date.ToString("s") + ".000Z";

        return $"Date={date}&WorkerActivityId={WorkerActivityId}";
    }
}