using HairdresserBookingApi.Models.Dto.Helper;

namespace HairdresserBookingApi.Models.Dto.Reservation;

public class ReservationRequirementDto
{
    public int WorkerActivityId { get; set; }
    public TimeRange TimeRange { get; set; }
}