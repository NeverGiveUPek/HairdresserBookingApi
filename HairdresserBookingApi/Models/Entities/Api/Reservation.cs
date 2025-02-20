﻿using System.ComponentModel.DataAnnotations;
using HairdresserBookingApi.Models.Entities.Users;

namespace HairdresserBookingApi.Models.Entities.Api;

public class Reservation
{
    [Required] public int Id { get; set; }
    [Required] public DateTime Date { get; set; }

    [Required] public int UserId { get; set; }
    public virtual User User { get; set; }

    [Required] public int WorkerActivityId { get; set; }
    public virtual WorkerActivity WorkerActivity { get; set; }
}