namespace VetClinic.Web.Models.Entities;

public class Veterinarian
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNo { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<VeterinarianSpecialty> Specialties { get; set; } = [];
    public ICollection<VeterinarianClinicService> Services { get; set; } = [];
    public ICollection<VeterinarianAvailability> Availabilities { get; set; } = [];
    public ICollection<VeterinarianTimeOff> TimeOffs { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
}
