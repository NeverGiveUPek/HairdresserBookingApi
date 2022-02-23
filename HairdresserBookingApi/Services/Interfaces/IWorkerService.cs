using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Worker;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IWorkerService
{
    List<WorkerDto> GetAll();
    WorkerDetailsDto GetById(int id);

}
