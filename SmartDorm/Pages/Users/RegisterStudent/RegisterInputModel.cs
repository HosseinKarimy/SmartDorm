using SmartDorm.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartDorm.Pages.Users.RegisterStudent;

public class RegisterInputModel
{
    // Account
    [Required, MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    // Student profile
    [Required, MaxLength(20)]
    public string StudentNumber { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public GenderType Gender { get; set; } = GenderType.Male;

    public int? BirthdayYear { get; set; }
    public FieldOfStudy FieldOfStudy { get; set; }

    public int EntryYear { get; set; } = DateTime.UtcNow.Year;

    [Required]
    public DegreeLevel DegreeLevel { get; set; } = DegreeLevel.Bachelor;

    [EmailAddress, MaxLength(100)]
    public string? Email { get; set; }

    [Phone, MaxLength(30)]
    public string? PhoneNumber { get; set; }

    // Preference
    [Required]
    public SleepHabit SleepHabit { get; set; }

    [Required]
    public NoiseToleranceLevel NoiseToleranceLevel { get; set; }

    [Required]
    public StudyStyle StudyStyle { get; set; }

    [Required]
    public RoomAtmosphere RoomAtmosphere { get; set; }

    [Range(1, 10)]
    public int RoomCapacity { get; set; } = 2;

    // One optional SpecialCondition (later extend to a list)
    public bool HasSpecialCondition { get; set; }

    public ConditionType ConditionType { get; set; } = ConditionType.Allergy;

    public string? ConditionDescription { get; set; }

    public Severity ConditionSeverity { get; set; } = Severity.Low;

    public string? ConditionNeeds { get; set; }
}
