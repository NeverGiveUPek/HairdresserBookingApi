using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IServiceService
{
    List<ServiceDto> GetAll();


}