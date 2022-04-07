using System;
using System.Collections.Generic;
using FluentAssertions;
using HairdresserBookingApi.Helpers;
using Xunit;

namespace HairdresserBookingApi.UnitTests;

public class DateTimeHelperTests
{
    public static IEnumerable<object[]> PresentDates()
    {
        yield return new object[]
        {
            DateTime.Now.AddDays(1)
        };
        yield return new object[]
        {
            DateTime.Now.AddMinutes(15)
        };

        yield return new object[]
        {
            DateTime.Now.Date
        };
    }

    public static IEnumerable<object[]> NotPresentDates()
    {
        yield return new object[]
        {
            DateTime.Now.AddDays(-2)
        };
        yield return new object[]
        {
            DateTime.Now.Date.AddMinutes(-15)
        };

    }

    public static IEnumerable<object[]> FutureDates()
    {
        yield return new object[]
        {
            DateTime.Now.AddDays(1)
        };
        yield return new object[]
        {
            DateTime.Now.AddMinutes(15)
        };
    }

    public static IEnumerable<object[]> NotFutureDates()
    {
        yield return new object[]
        {
            DateTime.Now.AddDays(-1)
        };
        yield return new object[]
        {
            DateTime.Now.AddMinutes(-15)
        };
    }

    public static IEnumerable<object[]> HasMinimumTimeSpanDates()
    {
        yield return new object[]
        {
            new DateTime(2030,1,1),
            5
        };

        yield return new object[]
        {
            new DateTime(2030,1,1,0,10,0),
            10
        };

        yield return new object[]
        {
            new DateTime(2030,1,1,0,10,0),
            5
        };

    }

    public static IEnumerable<object[]> WithoutMinimumTimeSpanDates()
    {
        yield return new object[]
        {
            new DateTime(2030,1,1,0,1,0),
            5
        };

        yield return new object[]
        {
            new DateTime(2030,1,1,0,0,1),
            10
        };

        yield return new object[]
        {
            new DateTime(2030,1,1,0,0,0,5),
            5
        };

    }


    [Theory]
    [MemberData(nameof(PresentDates))]
    public void IsDatePresent_ForGivenDate_ReturnsTrue(DateTime dateTime)
    {
        var result = DateTimeHelper.IsDatePresent(dateTime);

        result.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(NotPresentDates))]
    public void IsDatePresent_ForGivenDate_ReturnsFalse(DateTime dateTime)
    {
        var result = DateTimeHelper.IsDatePresent(dateTime);

        result.Should().BeFalse();
    }


    [Theory]
    [MemberData(nameof(FutureDates))]

    public void IsDateTimeInFuture_ForGivenDate_ReturnsTrue(DateTime dateTime)
    {
        var result = DateTimeHelper.IsDateTimeInFuture(dateTime);

        result.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(NotFutureDates))]
    public void IsDateTimeInFuture_ForGivenDate_ReturnsFalse(DateTime dateTime)
    {
        var result = DateTimeHelper.IsDateTimeInFuture(dateTime);

        result.Should().BeFalse();

    }

    [Theory]
    [MemberData(nameof(HasMinimumTimeSpanDates))]
    public void HasMinimumTimeSpanAsCertainMinutes_ForGivenDateAndSpan_ReturnsTrue(DateTime dateTime, int minutesSpan)
    {
        var result = DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(dateTime, minutesSpan);

        result.Should().BeTrue();

    }

    [Theory]
    [MemberData(nameof(WithoutMinimumTimeSpanDates))]
    public void HasMinimumTimeSpanAsCertainMinutes_ForGivenDateAndSpan_ReturnsFalse(DateTime dateTime, int minutesSpan)
    {
        var result = DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(dateTime, minutesSpan);

        result.Should().BeFalse();

    }


}