using HairdresserBookingApi.Models.Dto.Helper;

namespace HairdresserBookingApi.Services.Strategies;

public class EarliestReservationSelectorStrategy : IReservationSelectorStrategy
{
    public DateTime? FindBestTime(List<TimeRange> accessibility)
    {
        return accessibility.OrderBy(x => x.StartDate.TimeOfDay).FirstOrDefault()?.StartDate;
    }
}