using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SmartDorm.Models;

public class DormitoryManager
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    
    [MaxLength(15)]
    public required string PhoneNumber { get; set; }

    //Navigation
    public int UserId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;
}
