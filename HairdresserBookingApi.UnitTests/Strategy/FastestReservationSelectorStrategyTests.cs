using System;
using System.Collections.Generic;
using FluentAssertions;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Services.Strategies;
using HairdresserBookingApi.UnitTests.Strategy.Data;
using Xunit;

namespace HairdresserBookingApi.UnitTests.Strategy;

public class FastestReservationSelectorStrategyTests
{
    
    [ClassData(typeof(FastestReservationTestData))]
    [Theory]
    public void FindBestTime_ForCorrectTimeRanges_ReturnsBestTimeRange(List<TimeRange> accessibility, DateTime bestTime)
    {
        var strategy = new FastestReservationSelectorStrategy();

        var result = strategy.FindBestTime(accessibility);

        result.Should().Be(bestTime);
    }

    [Fact]
    public void FindBestTime_ForIncorrectAccessibility_ReturnsNull()
    {
        var accessibility = new List<TimeRange>();

        var strategy = new FastestReservationSelectorStrategy();

        var result = strategy.FindBestTime(accessibility);

        result.Should().BeNull();

    }

}