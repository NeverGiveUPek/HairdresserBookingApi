using AutoMapper;
using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Service, ServiceDto>();


    }


}