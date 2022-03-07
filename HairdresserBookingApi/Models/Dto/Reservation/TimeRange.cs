namespace HairdresserBookingApi.Models.Dto.Reservation;

public class TimeRange
{
    public TimeRange(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }
    public DateTime From { get; set; }
    public DateTime To { get; set; }

}