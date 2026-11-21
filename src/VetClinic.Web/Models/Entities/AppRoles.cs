namespace VetClinic.Web.Models.Entities;

/// <summary>Identity role names required by the assignment.</summary>
public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Veterinarian = "Veterinarian";
    public const string Member = "Member";

    public static readonly IReadOnlyList<string> All = [Admin, Veterinarian, Member];
}
