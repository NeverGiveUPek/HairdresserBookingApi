using AutoMapper;
using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Service, ServiceDto>();
        CreateMap<WorkerService, AvailableServiceDto>()
            .ForMember(m => m.Name, c => c.MapFrom(s => s.Service.Name))
            .ForMember(m => m.Description, c => c.MapFrom(s => s.Service.Description))
            .ForMember(m => m.Id, c => c.MapFrom(s => s.Service.Id))
            .ForMember(m => m.IsForMan, c => c.MapFrom(s => s.Service.IsForMan))
            .ForMember(m => m.MinPrice, c => c.MapFrom(s => s.Price));
    }
}