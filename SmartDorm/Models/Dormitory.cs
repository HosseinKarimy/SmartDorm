namespace SmartDorm.Models;

using System.ComponentModel.DataAnnotations;
using SmartDorm.Enums;


public class Dormitory
{
    public int DormitoryId { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(200)]
    public string? Address { get; set; }

    [MaxLength(100)]
    public string? CampusLocation { get; set; }

    public int TotalCapacity { get; set; }

    public GenderType GenderType { get; set; }

    // Not strictly needed, but helpful if you want "primary manager"
    public int? ManagerId { get; set; }
    public DormitoryManager? Manager { get; set; }

    public ICollection<Room> Rooms { get; set; } = [];
}
