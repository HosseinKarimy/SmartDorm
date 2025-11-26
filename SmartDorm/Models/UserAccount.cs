using System.ComponentModel.DataAnnotations;
using SmartDorm.Enums;

namespace SmartDorm.Models;

public class UserAccount
{
    public int Id { get; set; }

    [MaxLength(100)]
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.Student;

    public bool IsActive { get; set; } = true;

    // Navigation
    public Student? Student { get; set; }
    public DormitoryManager? DormitoryManager { get; set; }

}