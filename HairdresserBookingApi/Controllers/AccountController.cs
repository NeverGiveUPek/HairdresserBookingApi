using HairdresserBookingApi.Models.Dto.User;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairdresserBookingApi.Controllers;


[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{

    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }


    [HttpPost("register")]
    public ActionResult RegisterUser([FromBody] CreateUserDto dto)
    {
        _accountService.CreateUser(dto);

        return Ok();
    }


    [HttpPost("login")]
    public ActionResult LoginUser([FromBody] LoginUserDto dto)
    {
        var jwt = _accountService.GenerateUserJwt(dto);

        return Ok(jwt);
    }

    [HttpPut("{id}/role")]
    [Authorize("Admin,Manager")]
    public ActionResult ChangeUserRole([FromBody] string role, [FromRoute] int id)
    {
        _accountService.ChangeRole(role, id);

        return Ok();
    }
}