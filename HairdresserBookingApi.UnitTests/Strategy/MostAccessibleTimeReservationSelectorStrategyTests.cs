using System;
using System.Collections.Generic;
using FluentAssertions;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Services.Strategies;
using HairdresserBookingApi.UnitTests.Strategy.Data;
using Xunit;

namespace HairdresserBookingApi.UnitTests.Strategy;

public class MostAccessibleTimeReservationSelectorStrategyTests
{
    [ClassData(typeof(MostAccessibleTimeStrategyData))]
    [Theory]
    public void FindBestTime_ForCorrectTimeRanges_ReturnsBestTimeRange(List<TimeRange> accessibility, DateTime bestTime)
    {
        var strategy = new MostAccessibleTimeReservationSelectorStrategy();

        var result = strategy.FindBestTime(accessibility);

        result.Should().Be(bestTime);
    }

    [Fact]
    public void FindBestTime_ForIncorrectAccessibility_ReturnsNull()
    {
        var accessibility = new List<TimeRange>();

        var strategy = new MostAccessibleTimeReservationSelectorStrategy();

        var result = strategy.FindBestTime(accessibility);

        result.Should().BeNull();

    }
}