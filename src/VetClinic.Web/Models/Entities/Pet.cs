namespace VetClinic.Web.Models.Entities;

public class Pet
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public Owner Owner { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public Species Species { get; set; }
    public string? Breed { get; set; }
    public PetSex Sex { get; set; }
    public DateOnly? BirthDate { get; set; }
    public decimal? WeightKg { get; set; }
    public string? MicrochipNo { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<ExaminationRecord> ExaminationRecords { get; set; } = [];
    public ICollection<Vaccination> Vaccinations { get; set; } = [];
}
