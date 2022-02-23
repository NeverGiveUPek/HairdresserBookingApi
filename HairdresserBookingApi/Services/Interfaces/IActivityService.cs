using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Activity;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IActivityService
{
    List<ActivityDto> GetAll();
    List<AvailableActivityDto> GetAllAvailable();

    ActivityDetailsDto GetById(int id);

    //Returns id of created entity
    int Create(CreateActivityDto dto); 
     
    void Delete(int id);

    void Update(UpdateActivityDto dto, int id);

}