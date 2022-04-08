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

    public override string ToString()
    {
        var startDate = StartDate.ToString("s") + ".000Z";
        var endDate = EndDate.ToString("s") + ".000Z";


        return $"TimeRange.StartDate={startDate}&TimeRange.EndDate={endDate}";
    }
}