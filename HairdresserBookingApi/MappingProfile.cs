using AutoMapper;
using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Activity;
using HairdresserBookingApi.Models.Dto.Worker;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Activity, ActivityDto>();
        
        CreateMap<WorkerActivity, AvailableActivityDto>()
            .ForMember(m => m.Name, c => c.MapFrom(s => s.Activity.Name))
            .ForMember(m => m.Description, c => c.MapFrom(s => s.Activity.Description))
            .ForMember(m => m.Id, c => c.MapFrom(s => s.Activity.Id))
            .ForMember(m => m.IsForMan, c => c.MapFrom(s => s.Activity.IsForMan))
            .ForMember(m => m.MinPrice, c => c.MapFrom(s => s.Price));


        CreateMap<Worker, WorkerDto>();
        CreateMap<Worker, WorkerDetailsDto>();



    }
}