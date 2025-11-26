using SmartDorm.Enums;

namespace SmartDorm.Models;

public class StudentPreference
{
    public int PreferenceId { get; set; }
    public SleepHabit SleepHabit { get; set; }
    public NoiseToleranceLevel NoiseToleranceLevel { get; set; }
    public StudyStyle StudyStyle { get; set; }
    public RoomAtmosphere RoomAtmosphere { get; set; }
    public int RoomCapacity { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
}
