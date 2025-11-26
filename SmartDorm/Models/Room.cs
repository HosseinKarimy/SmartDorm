namespace SmartDorm.Models;

using SmartDorm.Enums;

public class Room
{
    public int RoomId { get; set; }
    public int DormitoryId { get; set; }
    public Dormitory Dormitory { get; set; } = null!;

    public int RoomNumber { get; set; }
    public int Floor { get; set; }
    public int WingOrBlock { get; set; }
    public int Capacity { get; set; }

    public bool HasPrivateBathroom { get; set; }
    public GenderType GenderType { get; set; }

    public bool IsActive { get; set; } = true;
}
