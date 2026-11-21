namespace VetClinic.Web.Models.Entities;

public class ExaminationRecord
{
    public int Id { get; set; }

    public int? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }

    public int PetId { get; set; }
    public Pet Pet { get; set; } = null!;

    public int VeterinarianId { get; set; }
    public Veterinarian Veterinarian { get; set; } = null!;

    public DateTime ExaminationDateUtc { get; set; }
    public string? Complaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Prescription { get; set; }
    public decimal? WeightKg { get; set; }
    public decimal? Temperature { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<Vaccination> Vaccinations { get; set; } = [];
}

public class Vaccination
{
    public int Id { get; set; }

    public int PetId { get; set; }
    public Pet Pet { get; set; } = null!;

    public int VeterinarianId { get; set; }
    public Veterinarian Veterinarian { get; set; } = null!;

    public int? ExaminationRecordId { get; set; }
    public ExaminationRecord? ExaminationRecord { get; set; }

    public string VaccineName { get; set; } = string.Empty;
    public string? BatchNumber { get; set; }
    public DateTime AdministeredDateUtc { get; set; }
    public DateOnly? NextDueDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
