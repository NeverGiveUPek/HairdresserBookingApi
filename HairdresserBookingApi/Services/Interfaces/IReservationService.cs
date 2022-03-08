using HairdresserBookingApi.Models.Dto.Reservation;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IReservationService
{
    List<TimeRange> GetAllPossibleTimesInDay(ReservationDto request);

}