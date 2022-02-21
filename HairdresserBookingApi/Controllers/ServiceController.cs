using AutoMapper;
using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;


[ApiController]
[Route("api/services")]
public class ServiceController : ControllerBase
{

    private readonly IServiceService _serviceService;
    


    public ServiceController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ServiceDto>> GetAll()
    {
        var allServices = _serviceService.GetAll();


        return Ok(allServices);
    }

}