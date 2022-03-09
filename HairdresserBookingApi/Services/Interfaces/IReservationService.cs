using HairdresserBookingApi.Models.Dto.Reservation;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IReservationService
{
    List<TimeRange> GetAllPossibleTimesInDay(ReservationRequestDto request);

    void MakeReservation(ReservationRequestDto request);

}