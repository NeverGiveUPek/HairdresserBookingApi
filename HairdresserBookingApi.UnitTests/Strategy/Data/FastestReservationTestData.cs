using System;
using System.Collections;
using System.Collections.Generic;
using HairdresserBookingApi.Models.Dto.Helper;

namespace HairdresserBookingApi.UnitTests.Strategy.Data;

public class FastestReservationTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new List<TimeRange>()
            {
                new TimeRange(new DateTime(2030, 1, 1, 10, 0, 0), new DateTime(2030, 1, 1, 18, 0, 0)),
                new TimeRange(new DateTime(2030, 1, 2, 8, 0, 0), new DateTime(2030, 1, 2, 16, 0, 0)),
                new TimeRange(new DateTime(2030, 1, 3, 10, 0, 0), new DateTime(2030, 1, 3, 18, 0, 0))
            },
            new DateTime(2030, 1, 1, 10, 0, 0)
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

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}