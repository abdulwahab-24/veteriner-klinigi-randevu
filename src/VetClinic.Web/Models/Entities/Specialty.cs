namespace VetClinic.Web.Models.Entities;

public class Specialty
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<VeterinarianSpecialty> Veterinarians { get; set; } = [];
}

/// <summary>Many-to-many link between veterinarians and specialties.</summary>
public class VeterinarianSpecialty
{
    public int VeterinarianId { get; set; }
    public Veterinarian Veterinarian { get; set; } = null!;

    public int SpecialtyId { get; set; }
    public Specialty Specialty { get; set; } = null!;
}
