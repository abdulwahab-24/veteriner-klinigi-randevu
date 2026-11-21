namespace VetClinic.Web.Models.Entities;

public class Appointment
{
    public int Id { get; set; }

    public int PetId { get; set; }
    public Pet Pet { get; set; } = null!;

    public int VeterinarianId { get; set; }
    public Veterinarian Veterinarian { get; set; } = null!;

    public int ServiceId { get; set; }
    public ClinicService Service { get; set; } = null!;

    /// <summary>Actual instants, always UTC. Convert to Europe/Istanbul for display.</summary>
    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public string? Notes { get; set; }
    public string? StatusReason { get; set; }

    // Snapshots taken at booking time so later service changes don't alter history.
    public string ServiceNameSnapshot { get; set; } = string.Empty;
    public int ServiceDurationMinutesSnapshot { get; set; }
    public decimal ServicePriceSnapshot { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public string CreatedByUserId { get; set; } = string.Empty;
    public ApplicationUser CreatedByUser { get; set; } = null!;

    public DateTime? StatusChangedAtUtc { get; set; }
    public string? StatusChangedByUserId { get; set; }
    public ApplicationUser? StatusChangedByUser { get; set; }

    public byte[] RowVersion { get; set; } = [];

    public ExaminationRecord? ExaminationRecord { get; set; }
}
