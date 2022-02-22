﻿using AutoMapper;
using HairdresserBookingApi.Models.Dto;
using HairdresserBookingApi.Models.Dto.Activity;
using HairdresserBookingApi.Services.Interfaces;
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
    public ActionResult<IEnumerable<ActivityDto>> GetAll()
    {
        var allActivities = _activityService.GetAll();


        return Ok(allActivities);
    }

    [HttpGet("available")]
    public ActionResult<IEnumerable<AvailableActivityDto>> GetAllAvailable()
    {
        var allActivities = _activityService.GetAllAvailable();

        return Ok(allActivities);
    }

}