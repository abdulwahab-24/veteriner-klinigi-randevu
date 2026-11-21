namespace VetClinic.Web.Infrastructure;

/// <summary>
/// Clinic time zone helpers. Appointment instants are stored in UTC; recurring weekly hours
/// are stored in local clinic time (Europe/Istanbul, UTC+3, no daylight saving since 2016).
/// </summary>
public static class ClinicTime
{
    public const string TimeZoneId = "Europe/Istanbul";

    public static TimeZoneInfo TimeZone { get; } = ResolveTimeZone();

    public static DateTime ToLocal(DateTime utc)
    {
        if (utc.Kind != DateTimeKind.Utc)
            throw new ArgumentException("Value must be a UTC DateTime.", nameof(utc));
        return TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZone);
    }

    public static DateTime ToUtc(DateTime local)
    {
        if (local.Kind == DateTimeKind.Utc)
            throw new ArgumentException("Value must be a local clinic time, not UTC.", nameof(local));
        return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(local, DateTimeKind.Unspecified), TimeZone);
    }

    private static TimeZoneInfo ResolveTimeZone()
    {
        // .NET resolves IANA ids on Windows via ICU; fall back to the Windows id explicitly.
        if (TimeZoneInfo.TryFindSystemTimeZoneById(TimeZoneId, out var tz))
            return tz;
        if (TimeZoneInfo.TryConvertIanaIdToWindowsId(TimeZoneId, out var windowsId) &&
            TimeZoneInfo.TryFindSystemTimeZoneById(windowsId, out tz))
            return tz;
        throw new InvalidOperationException($"Time zone '{TimeZoneId}' is not available on this system.");
    }
}
