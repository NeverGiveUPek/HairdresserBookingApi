using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Worker;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IWorkerService
{
    List<WorkerDto> GetAll();
    WorkerDetailsDto GetById(int id);
    int Create(CreateWorkerDto dto);
    void Update(UpdateWorkerDto dto, int id);
    void Delete(int id);
}
