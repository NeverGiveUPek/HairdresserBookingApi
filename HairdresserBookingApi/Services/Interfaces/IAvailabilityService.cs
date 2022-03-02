using HairdresserBookingApi.Models.Dto.Availability;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IAvailabilityService
{
    List<AvailabilityDto> GetAllAvailabilities(int workerId);
    List<AvailabilityDto> GetCurrentAvailabilities(int workerId);

    void AddAvailability(AddAvailabilityDto dto, int workerId);

}