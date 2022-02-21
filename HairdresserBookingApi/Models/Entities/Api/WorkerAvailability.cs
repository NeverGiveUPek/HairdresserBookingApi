namespace HairdresserBookingApi.Models.Entities.Api;

public class WorkerAvailability
{
    public int Id { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }


    public int WorkerId { get; set; }
    public virtual Worker Worker { get; set; }

    public virtual ICollection<WorkBreak> Breaks { get; set; }
}
