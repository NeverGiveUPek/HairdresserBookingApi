using HairdresserBookingApi.Models.Dto.Worker;

namespace HairdresserBookingApi.Models.Dto.WorkerActivity;

public class WorkerActivityDto
{
    public int Id { get; set; }
    public double Price { get; set; }
    public int RequiredMinutes { get; set; }
    public WorkerDto Worker { get; set; }
}