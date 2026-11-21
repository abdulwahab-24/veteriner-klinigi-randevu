using Microsoft.AspNetCore.Identity;

namespace VetClinic.Web.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}
