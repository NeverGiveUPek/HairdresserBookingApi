using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using TimeRange = HairdresserBookingApi.Models.Dto.Helper.TimeRange;

namespace HairdresserBookingApi.Services.Strategies;

public class FastestReservationSelectorStrategy : IReservationSelectorStrategy
{
    
    public DateTime? FindBestTime(List<TimeRange> accessibility)
    {
        var first = accessibility.FirstOrDefault();

        return first?.StartDate;
    }
}