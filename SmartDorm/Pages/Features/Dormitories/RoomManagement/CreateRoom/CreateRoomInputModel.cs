using SmartDorm.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmartDorm.Pages.Features.Dormitories.RoomManagement.CreateRoom;

public class CreateRoomInputModel
{
    [Required]
    [Display(Name = "Dormitory")]
    public int DormitoryId { get; set; }

    [Required]
    [Display(Name = "Room number")]
    public int RoomNumber { get; set; }

    [Display(Name = "Floor")]
    public int Floor { get; set; }

    [Display(Name = "Wing/Block")]
    public int WingOrBlock { get; set; }

    [Required]
    [Range(1, 10)]
    public int Capacity { get; set; }

    [Display(Name = "Has private bathroom")]
    public bool HasPrivateBathroom { get; set; }

    [Display(Name = "Gender type")]
    public GenderType GenderType { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}