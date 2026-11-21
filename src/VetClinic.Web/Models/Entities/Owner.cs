namespace VetClinic.Web.Models.Entities;

/// <summary>Pet-owner profile. Linked to an Identity user with the Member role (optional).</summary>
public class Owner
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<Pet> Pets { get; set; } = [];
}
