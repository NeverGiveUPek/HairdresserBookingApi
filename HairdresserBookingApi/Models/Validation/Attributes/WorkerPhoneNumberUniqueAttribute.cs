using System.ComponentModel.DataAnnotations;
using HairdresserBookingApi.Models.Db;

namespace HairdresserBookingApi.Models.Validation.Attributes;

public class WorkerPhoneNumberUniqueAttribute : ValidationAttribute
{
    public string GetErrorMessage(string? phoneNumber) =>
        $"PhoneNumber {phoneNumber} is already in use";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;


        var context = (BookingDbContext) validationContext.GetService(typeof(BookingDbContext))!;
        var entity = context.Workers.SingleOrDefault(e => e.PhoneNumber == value.ToString());


        if (entity != null)
        {
            return new ValidationResult(GetErrorMessage(value.ToString()));
        }


        return ValidationResult.Success;
    }
}