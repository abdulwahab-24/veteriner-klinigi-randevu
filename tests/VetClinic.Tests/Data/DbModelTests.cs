using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using VetClinic.Web.Data;
using VetClinic.Web.Models.Entities;

namespace VetClinic.Tests.Data;

/// <summary>
/// Verifies the EF Core model configuration without connecting to a database.
/// </summary>
public class DbModelTests
{
    private static VetClinicDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<VetClinicDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VetClinic_ModelOnly;Trusted_Connection=True")
            .Options);

    private static IModel DesignModel()
    {
        using var ctx = CreateContext();
        return ctx.GetService<IDesignTimeModel>().Model;
    }

    private static IEntityType Entity<T>() => DesignModel().FindEntityType(typeof(T))!;

    [Fact]
    public void CreateScript_IsGeneratedForAllTables()
    {
        using var ctx = CreateContext();
        var sql = ctx.Database.GenerateCreateScript();

        foreach (var table in new[]
                 {
                     "AspNetUsers", "AspNetRoles", "Owners", "Pets", "Veterinarians", "Specialties",
                     "VeterinarianSpecialties", "Services", "VeterinarianServices", "ClinicWorkingHours",
                     "VeterinarianAvailabilities", "VeterinarianTimeOffs", "Appointments",
                     "ExaminationRecords", "Vaccinations", "McpAccessTokens"
                 })
        {
            Assert.Contains($"CREATE TABLE [{table}]", sql);
        }
    }

    [Fact]
    public void Appointment_HasRowVersion_StringStatus_AndPrecisePriceSnapshot()
    {
        var appt = Entity<Appointment>();

        var rowVersion = appt.FindProperty(nameof(Appointment.RowVersion))!;
        Assert.True(rowVersion.IsConcurrencyToken);
        Assert.Equal(ValueGenerated.OnAddOrUpdate, rowVersion.ValueGenerated);

        Assert.Equal(typeof(string), appt.FindProperty(nameof(Appointment.Status))!.GetProviderClrType());

        var price = appt.FindProperty(nameof(Appointment.ServicePriceSnapshot))!;
        Assert.Equal(10, price.GetPrecision());
        Assert.Equal(2, price.GetScale());
    }

    [Fact]
    public void Appointment_HasRangeAndSnapshotCheckConstraints()
    {
        var names = Entity<Appointment>().GetCheckConstraints().Select(c => c.Name).ToList();

        Assert.Contains("CK_Appointments_Range", names);
        Assert.Contains("CK_Appointments_DurationSnapshot", names);
        Assert.Contains("CK_Appointments_PriceSnapshot", names);
    }

    [Fact]
    public void Appointment_BlockingUniqueIndex_IsFilteredToPendingAndApproved()
    {
        var index = Entity<Appointment>().GetIndexes()
            .Single(i => i.GetDatabaseName() == "UX_Appointments_Veterinarian_Start_Blocking");

        Assert.True(index.IsUnique);
        Assert.Equal("[Status] IN (N'Pending', N'Approved')", index.GetFilter());
    }

    [Fact]
    public void Appointment_HasOverlapLookupIndexesForVeterinarianAndPet()
    {
        var names = Entity<Appointment>().GetIndexes().Select(i => i.GetDatabaseName()).ToList();

        Assert.Contains("IX_Appointments_Veterinarian_Start", names);
        Assert.Contains("IX_Appointments_Pet_Start", names);
    }

    [Fact]
    public void ServicePrice_UsesDecimal10_2()
    {
        var price = Entity<ClinicService>().FindProperty(nameof(ClinicService.Price))!;
        Assert.Equal(10, price.GetPrecision());
        Assert.Equal(2, price.GetScale());
    }

    [Theory]
    [InlineData(typeof(Veterinarian), nameof(Veterinarian.LicenseNo))]
    [InlineData(typeof(Veterinarian), nameof(Veterinarian.UserId))]
    [InlineData(typeof(Owner), nameof(Owner.UserId))]
    [InlineData(typeof(Pet), nameof(Pet.MicrochipNo))]
    [InlineData(typeof(ClinicService), nameof(ClinicService.Name))]
    [InlineData(typeof(Specialty), nameof(Specialty.Name))]
    [InlineData(typeof(ClinicWorkingHour), nameof(ClinicWorkingHour.DayOfWeek))]
    [InlineData(typeof(ExaminationRecord), nameof(ExaminationRecord.AppointmentId))]
    [InlineData(typeof(McpAccessToken), nameof(McpAccessToken.TokenHash))]
    public void UniqueIndexes_AreConfigured(Type entityType, string property)
    {
        var entity = DesignModel().FindEntityType(entityType)!;

        Assert.Contains(entity.GetIndexes(), i =>
            i.IsUnique && i.Properties.Count == 1 && i.Properties[0].Name == property);
    }

    [Theory]
    [InlineData(typeof(VeterinarianSpecialty), nameof(VeterinarianSpecialty.VeterinarianId), nameof(VeterinarianSpecialty.SpecialtyId))]
    [InlineData(typeof(VeterinarianClinicService), nameof(VeterinarianClinicService.VeterinarianId), nameof(VeterinarianClinicService.ServiceId))]
    public void LinkTables_UseCompositeKeys(Type entityType, string first, string second)
    {
        var key = DesignModel().FindEntityType(entityType)!.FindPrimaryKey()!;

        Assert.Equal([first, second], key.Properties.Select(p => p.Name));
    }

    [Fact]
    public void DomainForeignKeys_UseRestrict_ExceptLinkTablesAndTokens()
    {
        var cascadeAllowed = new[]
        {
            typeof(VeterinarianSpecialty), typeof(VeterinarianClinicService), typeof(McpAccessToken)
        };

        var domainForeignKeys = DesignModel().GetEntityTypes()
            .Where(e => e.ClrType.Namespace == typeof(Appointment).Namespace
                        && e.ClrType != typeof(ApplicationUser)
                        && !cascadeAllowed.Contains(e.ClrType))
            .SelectMany(e => e.GetForeignKeys())
            .ToList();

        Assert.NotEmpty(domainForeignKeys);
        Assert.All(domainForeignKeys, fk =>
            Assert.True(fk.DeleteBehavior == DeleteBehavior.Restrict,
                $"{fk.DeclaringEntityType.ClrType.Name} -> {fk.PrincipalEntityType.ClrType.Name} is {fk.DeleteBehavior}"));
    }

    [Fact]
    public void UtcDateTimeColumns_HaveUtcKindConverter()
    {
        var utcProperties = DesignModel().GetEntityTypes()
            .SelectMany(e => e.GetProperties())
            .Where(p => p.Name.EndsWith("Utc") && (p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            .ToList();

        Assert.NotEmpty(utcProperties);
        Assert.All(utcProperties, p => Assert.NotNull(p.GetValueConverter()));

        var converter = Entity<Appointment>().FindProperty(nameof(Appointment.StartUtc))!.GetValueConverter()!;
        var fromDb = (DateTime)converter.ConvertFromProvider(new DateTime(2026, 11, 20, 10, 0, 0))!;
        Assert.Equal(DateTimeKind.Utc, fromDb.Kind);
    }

    [Fact]
    public void McpTokenHash_IsFixedLength32Bytes()
    {
        var hash = Entity<McpAccessToken>().FindProperty(nameof(McpAccessToken.TokenHash))!;
        Assert.Equal(32, hash.GetMaxLength());
        Assert.True(hash.IsFixedLength());
    }

    [Fact]
    public void Roles_MatchAssignment()
    {
        Assert.Equal(["Admin", "Veterinarian", "Member"], AppRoles.All);
    }

    [Fact]
    public void NewAppointment_DefaultsToPending()
    {
        Assert.Equal(AppointmentStatus.Pending, new Appointment().Status);
    }
}
