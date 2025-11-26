using System.ComponentModel.DataAnnotations;

namespace SmartDorm.Models;

public class DormitoryManager
{
    public int ManagerId { get; set; }
    public required string FullName { get; set; }
    
    [MaxLength(15)]
    public required string PhoneNumber { get; set; }

    //Navigation
    public int UserId { get; set; }
    public UserAccount UserAccount { get; set; } = null!;
}
