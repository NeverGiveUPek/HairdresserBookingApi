using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HairdresserBookingApi.Services.Strategies;

public class MostAccessibleTimeReservationSelectorStrategy : IReservationSelectorStrategy
{
    public DateTime? FindBestTime(List<TimeRange> accessibility)
    {
        return accessibility.OrderByDescending(x => x.EndDate - x.StartDate).FirstOrDefault()?.StartDate;
    }
}