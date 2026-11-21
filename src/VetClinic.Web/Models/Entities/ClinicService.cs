namespace VetClinic.Web.Models.Entities;

/// <summary>
/// A bookable clinic service (examination, vaccination, surgery...). Stored in the "Services" table.
/// Named ClinicService to avoid confusion with the application service layer.
/// </summary>
public class ClinicService
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<VeterinarianClinicService> Veterinarians { get; set; } = [];
}

/// <summary>
/// Many-to-many link: a veterinarian can only be booked for services listed here.
/// Stored in the "VeterinarianServices" table.
/// </summary>
public class VeterinarianClinicService
{
    public int VeterinarianId { get; set; }
    public Veterinarian Veterinarian { get; set; } = null!;

    public int ServiceId { get; set; }
    public ClinicService Service { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}
