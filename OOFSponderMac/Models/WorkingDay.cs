using System;
using System.Globalization;

namespace OOFSponderMac.Models;

/// <summary>
/// Represents one day's working-hours configuration.
/// Ported from the Windows OOFScheduling.WorkingDay class.
/// </summary>
public class WorkingDay
{
    private DateTime _startTime;
    private DateTime _endTime;

    public DayOfWeek DayOfWeek { get; set; }

    /// <summary>True = this day is marked as "off work" (OOF active).</summary>
    public bool IsOOF { get; set; }

    public DateTime StartTime
    {
        get => _startTime.EquivalentDateTime();
        set => _startTime = value;
    }

    public DateTime EndTime
    {
        get => _endTime.EquivalentDateTime();
        set => _endTime = value;
    }
}

public static class DateTimeExtensions
{
    /// <summary>
    /// Returns a DateTime that represents the same time-of-day
    /// as <paramref name="source"/> but anchored to the current week's
    /// equivalent day-of-week.
    /// </summary>
    public static DateTime EquivalentDateTime(this DateTime source)
    {
        int targetDow = (int)source.DayOfWeek;
        int currentDow = (int)DateTime.Today.DayOfWeek;
        return DateTime.Today
            .AddDays(targetDow - currentDow)
            .AddHours(source.Hour)
            .AddMinutes(source.Minute);
    }
}
