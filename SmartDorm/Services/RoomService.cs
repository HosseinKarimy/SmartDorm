using Microsoft.EntityFrameworkCore;
using SmartDorm.Data;
using SmartDorm.Enums;
using SmartDorm.Models;
using SmartDorm.Pages.Features.Dormitories.RoomManagement.CreateRoom;

namespace SmartDorm.Services;

public class RoomService(AppDbContext context) : IRoomService
{
    public async Task<List<Dormitory>> GetAllowedDormitoriesAsync(int userId, string role)
    {
        if (role == UserRole.Admin.ToString())
        {
            return await context.Dormitories
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        if (role == UserRole.DormitoryManager.ToString())
        {
            var dmId = context.DormitoryManagers.First(dm => dm.UserId == userId);

            return await context.Dormitories
                .Where(d => dmId.Id==d.ManagerId)
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        // Other roles: no dorms
        return [];
    }

    public async Task<Room> AddRoomAsync(CreateRoomInputModel input, int userId, string role)
    {
        // Ensure dormitory exists
        var dormitory = await context.Dormitories
            .FirstOrDefaultAsync(d => d.DormitoryId == input.DormitoryId);

        if (dormitory == null)
        {
            throw new InvalidOperationException("Dormitory not found.");
        }

        // Authorization per dormitory
        if (role == UserRole.DormitoryManager.ToString())
        {
            bool ownsDorm = await context.DormitoryManagers
                .AnyAsync(dm => dm.UserId == userId);

            if (!ownsDorm)
            {
                throw new UnauthorizedAccessException("You are not allowed to add rooms to this dormitory.");
            }
        }
        else if (role != UserRole.Admin.ToString())
        {
            throw new UnauthorizedAccessException("Only admins or dormitory managers can add rooms.");
        }

        // Map input to Room entity
        var room = new Room
        {
            DormitoryId = input.DormitoryId,
            RoomNumber = input.RoomNumber,
            Floor = input.Floor,
            WingOrBlock = input.WingOrBlock,
            Capacity = input.Capacity,
            HasPrivateBathroom = input.HasPrivateBathroom,
            GenderType = input.GenderType,
            IsActive = input.IsActive
        };

        context.Rooms.Add(room);
        await context.SaveChangesAsync();

        return room;
    }
}

public interface IRoomService
{
    /// <summary>
    /// Returns the dormitories the given user is allowed to manage/add rooms to.
    /// Admin: all dormitories. DormitoryManager: only their own dorm(s).
    /// </summary>
    Task<List<Dormitory>> GetAllowedDormitoriesAsync(int userId, string role);

    /// <summary>
    /// Adds a room to a dormitory if the user has permission.
    /// Throws InvalidOperationException or UnauthorizedAccessException on invalid state.
    /// </summary>
    Task<Room> AddRoomAsync(CreateRoomInputModel input, int userId, string role);
}