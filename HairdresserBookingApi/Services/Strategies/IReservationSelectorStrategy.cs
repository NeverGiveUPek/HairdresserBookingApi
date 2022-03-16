using AutoMapper.Configuration.Conventions;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Entities.Api;
using TimeRange = HairdresserBookingApi.Models.Dto.Helper.TimeRange;

namespace HairdresserBookingApi.Services.Strategies;

public interface IReservationSelectorStrategy
{
    DateTime? FindBestTime(List<TimeRange> accessibility);

}