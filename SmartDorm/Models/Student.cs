using System.ComponentModel.DataAnnotations;
using SmartDorm.Enums;

namespace SmartDorm.Models;

public class Student
{
    public int StudentId { get; set; }

    [MaxLength(20)]
    public required string StudentNumber { get; set; }

    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; } 

    public GenderType Gender { get; set; }

    public int? BirthdayYear { get; set; }

    public FieldOfStudy FieldOfStudy { get; set; }

    public int EntryYear { get; set; }

    public DegreeLevel DegreeLevel { get; set; }

    [EmailAddress, MaxLength(100)]
    public string? Email { get; set; }

    [Phone, MaxLength(30)]
    public string? PhoneNumber { get; set; }

    // Navigation

    public int UserId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;

    public StudentPreference? Preference { get; set; }
    public ICollection<SpecialCondition> SpecialConditions { get; set; } = [];
}