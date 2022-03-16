namespace HairdresserBookingApi.Models.Dto.Helper;

public class TimeRange
{
    public TimeRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public TimeRange()
    {
        
    }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}