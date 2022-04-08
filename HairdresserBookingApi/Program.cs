using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using HairdresserBookingApi.Middleware;
using HairdresserBookingApi.Models.Authentication;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Dto.User;
using HairdresserBookingApi.Models.Entities.Users;
using HairdresserBookingApi.Models.Validation.Validators;
using HairdresserBookingApi.Models.Validation.Validators.Availability;
using HairdresserBookingApi.Models.Validation.Validators.Reservation;
using HairdresserBookingApi.Models.Validation.Validators.User;
using HairdresserBookingApi.Seeders;
using HairdresserBookingApi.Services.Implementations;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

//authentication
var authSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authSettings);

builder.Services.AddSingleton(authSettings);
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = "Bearer";
    opt.DefaultScheme = "Bearer";
    opt.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(config =>
{
    config.SaveToken = true;
    config.RequireHttpsMetadata = false;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = authSettings.JwtIssuer,
        ValidIssuer = authSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.JwtKey))
    };
});


builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookingDbContext")));

builder.Services.AddScoped<SeederFacade>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("client", build =>
        build.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["AllowedOrigins"])
    );
});


builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IWorkerService, WorkerService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IWorkerActivityService, WorkerActivityService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<ExceptionMiddleware>();

builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
builder.Services.AddScoped<IValidator<LoginUserDto>, LoginUserDtoValidator>();
builder.Services.AddScoped<IValidator<AddAvailabilityDto>, AddAvailabilityDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateAvailabilityDto>, UpdateAvailabilityDtoValidator>();
builder.Services.AddScoped<IValidator<ReservationRequestDto>, ReservationRequestDtoValidator>();
builder.Services.AddScoped<IValidator<EditReservationDateDto>, EditReservationDateDtoValidator>();
builder.Services.AddScoped<IValidator<ReservationRequirementDto>, ReservationRequirementDtoValidator>();
builder.Services.AddScoped<IValidator<TimeRangeAvailabilityDto>, TimeRangeAvailabilityDtoValidator>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddHttpContextAccessor();

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();


var app = builder.Build();

app.UseCors("client");

var scope = app.Services.CreateScope();

var seeder = scope.ServiceProvider.GetRequiredService<SeederFacade>();

seeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}