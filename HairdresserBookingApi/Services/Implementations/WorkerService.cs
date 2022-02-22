using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Worker;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;

namespace HairdresserBookingApi.Services.Implementations;

public class WorkerService : IWorkerService
{
    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;


    public WorkerService(BookingDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public List<WorkerDto> GetAll()
    {
        var workers = _dbContext
            .Workers
            .ToList();

        var workersDto = _mapper.Map<List<WorkerDto>>(workers);

        return workersDto;
    }

    public WorkerDetailsDto GetById(int id)
    {
        var worker = _dbContext
            .Workers
            .SingleOrDefault(w => w.Id == id);

        if (worker == null) throw new NotFoundException($"Worker of id: {id} is not found");

        var workerDetailsDto = _mapper.Map<WorkerDetailsDto>(worker);

        return workerDetailsDto;
    }
}