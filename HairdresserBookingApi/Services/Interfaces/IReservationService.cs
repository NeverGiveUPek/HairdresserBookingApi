using HairdresserBookingApi.Models.Dto.Reservation;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IReservationService
{
    bool IsAccessible(ReservationRequestDto request);

    List<TimeRange> AccessibilityInDay(DateTime date,int workerId);

}