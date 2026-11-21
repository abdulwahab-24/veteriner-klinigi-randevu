using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VetClinic.Web.Models.Entities;

namespace VetClinic.Web.Data;

public class VetClinicDbContext(DbContextOptions<VetClinicDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Veterinarian> Veterinarians => Set<Veterinarian>();
    public DbSet<Specialty> Specialties => Set<Specialty>();
    public DbSet<VeterinarianSpecialty> VeterinarianSpecialties => Set<VeterinarianSpecialty>();
    public DbSet<ClinicService> ClinicServices => Set<ClinicService>();
    public DbSet<VeterinarianClinicService> VeterinarianClinicServices => Set<VeterinarianClinicService>();
    public DbSet<ClinicWorkingHour> ClinicWorkingHours => Set<ClinicWorkingHour>();
    public DbSet<VeterinarianAvailability> VeterinarianAvailabilities => Set<VeterinarianAvailability>();
    public DbSet<VeterinarianTimeOff> VeterinarianTimeOffs => Set<VeterinarianTimeOff>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<ExaminationRecord> ExaminationRecords => Set<ExaminationRecord>();
    public DbSet<Vaccination> Vaccinations => Set<Vaccination>();
    public DbSet<McpAccessToken> McpAccessTokens => Set<McpAccessToken>();

    // Values read back from SQL Server have DateTimeKind.Unspecified; mark *Utc columns as UTC.
    private static readonly ValueConverter<DateTime, DateTime> UtcConverter =
        new(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

    private static readonly ValueConverter<DateTime?, DateTime?> NullableUtcConverter =
        new(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Identity tables
        builder.ApplyConfigurationsFromAssembly(typeof(VetClinicDbContext).Assembly);

        foreach (var entity in builder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (!property.Name.EndsWith("Utc", StringComparison.Ordinal)) continue;

                if (property.ClrType == typeof(DateTime))
                    property.SetValueConverter(UtcConverter);
                else if (property.ClrType == typeof(DateTime?))
                    property.SetValueConverter(NullableUtcConverter);
            }
        }
    }
}
