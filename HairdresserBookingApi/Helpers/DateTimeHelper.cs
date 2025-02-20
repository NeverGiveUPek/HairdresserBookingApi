﻿namespace HairdresserBookingApi.Helpers;

public static class DateTimeHelper
{
    public static bool IsDatePresent(DateTime dateTime)
    {
        return dateTime.Date >= DateTime.Now.Date;
    }

    public static bool IsDateTimeInFuture(DateTime dateTime)
    {
        return dateTime > DateTime.Now;
    }

    public static bool HasMinimumTimeSpanAsCertainMinutes(DateTime dateTime, int minuteSpan)
    {
        if (dateTime.Millisecond != 0) return false;
        if (dateTime.Second != 0) return false;
        if (dateTime.Minute % minuteSpan != 0) return false;
        return true;
    }
}