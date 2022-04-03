using System;
using System.Collections.Generic;
using FluentAssertions;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Services.Strategies;
using Xunit;

namespace HairdresserBookingApi.UnitTests.Strategy;

public class EarliestReservationSelectorStrategyTests
{

    public static IEnumerable<object[]> GetSampleAccessibility()
    {
        yield return new object[]
        {
            new List<TimeRange>()
            {
                new TimeRange(new DateTime(2030, 1, 1, 10, 0, 0), new DateTime(2030, 1, 1, 18, 0, 0)),
                new TimeRange(new DateTime(2030, 1, 2, 8, 0, 0), new DateTime(2030, 1, 2, 16, 0, 0)),
                new TimeRange(new DateTime(2030, 1, 3, 10, 0, 0), new DateTime(2030, 1, 3, 18, 0, 0))
            },
            new DateTime(2030, 1, 2, 8, 0, 0)
        };

        yield return new object[]
        {
            new List<TimeRange>()
            {
                new TimeRange(new DateTime(2030, 1, 1, 8, 0, 0), new DateTime(2030, 1, 1, 18, 0, 0)),
                new TimeRange(new DateTime(2030, 1, 2, 8, 0, 0), new DateTime(2030, 1, 2, 16, 0, 0)),
                new TimeRange(new DateTime(2030, 1, 3, 10, 0, 0), new DateTime(2030, 1, 3, 18, 0, 0))
            },
            new DateTime(2030, 1, 1, 8, 0, 0)
        };

        yield return new object[]
        {
            new List<TimeRange>()
            {
                new TimeRange(new DateTime(2030, 1, 1, 10, 0, 0), new DateTime(2030, 1, 1, 12, 0, 0)),
                new TimeRange(new DateTime(2030, 1, 2, 14, 0, 0), new DateTime(2030, 1, 2, 18, 0, 0)),
            },
            new DateTime(2030, 1, 1, 10, 0, 0)
        };

        yield return new object[]
        {
            new List<TimeRange>()
            {
                new TimeRange(new DateTime(2030, 1, 1, 10, 0, 0), new DateTime(2030, 1, 1, 12, 0, 0)),
            },
            new DateTime(2030, 1, 1, 10, 0, 0)
        };
    }


    [MemberData(nameof(GetSampleAccessibility))]
    [Theory]
    public void FindBestTime_ForCorrectTimeRanges_ReturnsBestTimeRange(List<TimeRange> accessibility, DateTime bestTime)
    {
        var strategy = new EarliestReservationSelectorStrategy();

        var result = strategy.FindBestTime(accessibility);

        result.Should().Be(bestTime);

    }

    [Fact]
    public void FindBestTime_ForIncorrectAccessibility_ReturnsNull()
    {
        var accessibility = new List<TimeRange>();

        var strategy = new EarliestReservationSelectorStrategy();

        var result = strategy.FindBestTime(accessibility);

        result.Should().BeNull();

    }



}