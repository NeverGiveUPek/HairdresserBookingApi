using HairdresserBookingApi.Models.Dto.Helper;

namespace HairdresserBookingApi.Models.Dto.Reservation;

public class ReservationRequirementDto
{
    public int WorkerActivityId { get; set; }
    public TimeRange TimeRange { get; set; }

    public PickStrategy PickStrategy { get; set; } = Reservation.PickStrategy.Fast;

    public override string ToString()
    {
        return $"WorkerActivityId={WorkerActivityId}&{TimeRange.ToString()}&PickStrategy={(int)PickStrategy}";
    }
}