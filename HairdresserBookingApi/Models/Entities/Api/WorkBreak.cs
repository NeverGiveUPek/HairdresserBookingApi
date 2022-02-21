namespace HairdresserBookingApi.Models.Entities.Api;

public class WorkBreak
{
    public int Id { get; set; }

    public DateTime From { get; set; }
    public DateTime To { get; set; }

    public int WorkerAvailability { get; set; }
    public WorkerAvailability Availability { get; set; }
}