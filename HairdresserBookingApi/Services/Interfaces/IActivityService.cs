using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Activity;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IActivityService
{
    List<ActivityDto> GetAll();
    List<AvailableActivityDto> GetAllAvailable();
    void Create(CreateActivityDto dto);


}