namespace VetClinic.Web.Models.Entities;

/// <summary>Recurring weekly clinic opening hours, in local clinic time (Europe/Istanbul).</summary>
public class ClinicWorkingHour
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly OpenTime { get; set; }
    public TimeOnly CloseTime { get; set; }
    public bool IsClosed { get; set; }
}

/// <summary>Recurring weekly veterinarian working hours, in local clinic time (Europe/Istanbul).</summary>
public class VeterinarianAvailability
{
    public int Id { get; set; }
    public int VeterinarianId { get; set; }
    public Veterinarian Veterinarian { get; set; } = null!;

    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

/// <summary>A concrete period when a veterinarian is unavailable. Stored as UTC instants.</summary>
public class VeterinarianTimeOff
{
    public int Id { get; set; }
    public int VeterinarianId { get; set; }
    public Veterinarian Veterinarian { get; set; } = null!;

    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
    public string? Reason { get; set; }
}
