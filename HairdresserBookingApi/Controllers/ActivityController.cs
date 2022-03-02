using AutoMapper;
using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Activity;
using HairdresserBookingApi.Models.Entities.Users;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;


[ApiController]
[Route("api/activity")]
public class ActivityController : ControllerBase
{

    private readonly IActivityService _activityService;
    


    public ActivityController(IActivityService activityService)
    {
        _activityService = activityService;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<IEnumerable<ActivityDto>> GetAll()
    {
        var allActivities = _activityService.GetAll();


        return Ok(allActivities);
    }

    [HttpGet("available")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<AvailableActivityDto>> GetAllAvailable()
    {
        var allActivities = _activityService.GetAllAvailable();

        return Ok(allActivities);
    }

    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<ActivityDetailsDto> GetById([FromRoute] int id)
    {
        var activityDetailsDto = _activityService.GetById(id);

        return Ok(activityDetailsDto);
    }


    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult Create([FromBody] CreateActivityDto dto)
    {
        var id = _activityService.Create(dto);

        return Created($"api/activity/{id}", null);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult Delete([FromRoute] int id)
    {
        _activityService.Delete(id);

        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult Update(UpdateActivityDto dto, [FromRoute] int id)
    {
        _activityService.Update(dto, id);

        return Ok();
    }

}