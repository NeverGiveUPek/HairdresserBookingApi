using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Services.Interfaces;

namespace HairdresserBookingApi.Services.Implementations;


public class ServiceService : IServiceService
{

    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;

    public ServiceService(BookingDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public List<ServiceDto> GetAll()
    {
        var services = _dbContext
            .Services
            .ToList();

        var servicesDto = _mapper.Map<List<ServiceDto>>(services);

        return servicesDto;
    }

}