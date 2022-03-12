using AutoMapper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Worker;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public int Create(CreateWorkerDto dto)
    {

        var worker = _mapper.Map<Worker>(dto);

        _dbContext.Workers.Add(worker);
        _dbContext.SaveChanges();

        return worker.Id;
    }

    public void Update(UpdateWorkerDto dto, int id)
    {
        var foundWorker = _dbContext
            .Workers
            .FirstOrDefault(w => w.Id == id);

        if (foundWorker == null) throw new NotFoundException($"Worker of id: {id} is not found");

        foundWorker.FirstName = dto.FirstName;
        foundWorker.LastName = dto.LastName;
        foundWorker.Email = dto.Email;
        foundWorker.PhoneNumber = dto.PhoneNumber;

        _dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var foundWorker = _dbContext
            .Workers
            .Include(x => x.Availabilities)
            .Include(x => x.WorkerActivity)
            .ThenInclude(wa => wa.Reservations)
            .FirstOrDefault(w => w.Id == id);

        if (foundWorker == null) throw new NotFoundException($"Worker of id: {id} is not found");

        _dbContext.Workers.Remove(foundWorker);
        _dbContext.SaveChanges();
    }
}