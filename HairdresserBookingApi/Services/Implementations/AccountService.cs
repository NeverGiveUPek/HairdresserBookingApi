using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using HairdresserBookingApi.Models.Authentication;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.User;
using HairdresserBookingApi.Models.Entities.Users;
using HairdresserBookingApi.Models.Exceptions;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HairdresserBookingApi.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly BookingDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;

    public AccountService(IMapper mapper, BookingDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
    {
        _mapper = mapper;
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
    }


    public void CreateUser(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);

        var roleUser =  _dbContext.Roles.FirstOrDefault(r => r.Name == "User");

        if (roleUser == null) throw new Exception("Can't reach user role");

        user.RoleId = roleUser.Id;

        var passwordHash = _passwordHasher.HashPassword(user, dto.Password);

        user.PasswordHash = passwordHash;

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public string GenerateUserJwt(LoginUserDto dto)
    {
        var user = _dbContext
            .Users
            .Include(u => u.Role)
            .SingleOrDefault(u => u.Email == dto.Email);

        if (user == null) throw new AppException("Email or Password is incorrect");

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        
        if(passwordVerificationResult == PasswordVerificationResult.Failed) throw new AppException("Email or Password is incorrect");

        var claimList = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, $"{user.Role.Name}")
        };

        if (user.DateOfBirth != null)
        {
            claimList.Add(new Claim("DateOfBirth", user.DateOfBirth.Value.ToShortDateString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer,
            claimList, null, expires, credentials);

        var tokenHandler = new JwtSecurityTokenHandler();

        var generatedToken = tokenHandler.WriteToken(token);

        return generatedToken;
    }
}