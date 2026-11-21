using VetClinic.Web.Infrastructure;

namespace VetClinic.Tests.Infrastructure;

public class ClinicTimeTests
{
    [Fact]
    public void IstanbulTimeZone_IsAvailable()
    {
        Assert.NotNull(ClinicTime.TimeZone);
    }

    [Theory]
    [InlineData(2026, 1, 15)]  // winter
    [InlineData(2026, 7, 15)]  // summer — Türkiye has no DST since 2016
    [InlineData(2026, 11, 20)]
    public void Istanbul_IsAlwaysUtcPlus3(int year, int month, int day)
    {
        var utc = new DateTime(year, month, day, 7, 0, 0, DateTimeKind.Utc);

        Assert.Equal(TimeSpan.FromHours(3), ClinicTime.TimeZone.GetUtcOffset(utc));
        Assert.Equal(new DateTime(year, month, day, 10, 0, 0), ClinicTime.ToLocal(utc));
    }

    [Fact]
    public void LocalToUtc_RoundTrips()
    {
        var local = new DateTime(2026, 11, 20, 9, 30, 0);
        var utc = ClinicTime.ToUtc(local);

        Assert.Equal(DateTimeKind.Utc, utc.Kind);
        Assert.Equal(new DateTime(2026, 11, 20, 6, 30, 0, DateTimeKind.Utc), utc);
        Assert.Equal(local, ClinicTime.ToLocal(utc));
    }

    [Fact]
    public void LocalMidnight_MapsToPreviousUtcDay()
    {
        var utc = ClinicTime.ToUtc(new DateTime(2026, 11, 21, 0, 0, 0));
        Assert.Equal(new DateTime(2026, 11, 20, 21, 0, 0, DateTimeKind.Utc), utc);
    }

    [Fact]
    public void ToLocal_RejectsNonUtcInput()
    {
        Assert.Throws<ArgumentException>(() => ClinicTime.ToLocal(new DateTime(2026, 11, 20, 10, 0, 0)));
    }

    [Fact]
    public void ToUtc_RejectsUtcInput()
    {
        Assert.Throws<ArgumentException>(() =>
            ClinicTime.ToUtc(new DateTime(2026, 11, 20, 10, 0, 0, DateTimeKind.Utc)));
    }
}
