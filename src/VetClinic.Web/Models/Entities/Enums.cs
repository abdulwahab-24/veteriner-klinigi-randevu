namespace VetClinic.Web.Models.Entities;

public enum Species
{
    Dog,
    Cat,
    Bird,
    Rabbit,
    Rodent,
    Reptile,
    Other
}

public enum PetSex
{
    Unknown,
    Male,
    Female
}

/// <summary>
/// Pending and Approved appointments block the veterinarian's and pet's time slot.
/// All other statuses release it.
/// </summary>
public enum AppointmentStatus
{
    Pending,
    Approved,
    Rejected,
    Completed,
    Cancelled,
    NoShow
}
