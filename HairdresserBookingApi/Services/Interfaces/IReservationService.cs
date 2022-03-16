using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Models.Dto.Reservation;


namespace HairdresserBookingApi.Services.Interfaces;

public interface IReservationService
{
    List<TimeRange> GetAllPossibleTimesInDay(ReservationRequestDto request);

    void MakeReservation(ReservationRequestDto request);

    List<ReservationInfoDto> GetAllUserReservations();
    List<ReservationInfoDto> GetFutureUserReservations();

    void DeleteReservation(int reservationId);

    void EditReservation(int reservationId, EditReservationDateDto editReservationDto);

    ReservationRequestDto FindBestReservation(ReservationRequirementDto requirement);
}