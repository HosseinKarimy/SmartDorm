using System.ComponentModel.DataAnnotations;
using SmartDorm.Enums;

namespace SmartDorm.Models;

public class SpecialCondition
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public ConditionType ConditionType { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public Severity Severity { get; set; }

    [MaxLength(500)]
    public string? Needs { get; set; }
}
