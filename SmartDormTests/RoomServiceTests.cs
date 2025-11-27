using Microsoft.EntityFrameworkCore;
using SmartDorm.Data;
using SmartDorm.Enums;
using SmartDorm.Models;
using SmartDorm.Pages.Features.Dormitories.RoomManagement.CreateRoom;
using SmartDorm.Services;

namespace SmartDormTests;


public class RoomServiceTests
{
    private AppDbContext CreateDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new AppDbContext(options);
    }

    private async Task SeedDormitoriesAndManagersAsync(AppDbContext context)
    {
        var dorm1 = new Dormitory
        {
            DormitoryId = 1,
            Name = "North Hall",
            CampusLocation = "North",
            TotalCapacity = 300,
            GenderType = GenderType.Male,
        };

        var dorm2 = new Dormitory
        {
            DormitoryId = 2,
            Name = "West Residence",
            CampusLocation = "West",
            TotalCapacity = 200,
            GenderType = GenderType.Female
        };

        context.Dormitories.AddRange(dorm1, dorm2);

        // manager user: id = 2, manages dorm 1
        var managerUser = new UserAccount
        {
            Id = 2,
            Username = "dormmanager",
            PasswordHash = "fake",
            Role = UserRole.DormitoryManager,
            IsActive = true
        };

        var manager = new DormitoryManager
        {
            Id = 1,
            UserId = managerUser.Id,
            FullName = "Alice Manager",
            PhoneNumber = "555-0000"
        };

        context.UserAccounts.Add(managerUser);
        context.DormitoryManagers.Add(manager);

        await context.SaveChangesAsync();
    }

    // ---------- GetAllowedDormitoriesAsync ----------

    [Fact]
    public async Task GetAllowedDormitoriesAsync_AdminGetsAllDorms()
    {
        // Arrange
        var dbName = nameof(GetAllowedDormitoriesAsync_AdminGetsAllDorms);
        using var context = CreateDbContext(dbName);
        await SeedDormitoriesAndManagersAsync(context);

        var service = new RoomService(context);

        // Act
        var result = await service.GetAllowedDormitoriesAsync(
            userId: 1,
            role: UserRole.Admin.ToString());

        // Assert
        Assert.Equal(2, result.Count);                // both dorms
        Assert.Contains(result, d => d.DormitoryId == 1);
        Assert.Contains(result, d => d.DormitoryId == 2);
    }

    [Fact]
    public async Task GetAllowedDormitoriesAsync_StudentGetsNone()
    {
        // Arrange
        var dbName = nameof(GetAllowedDormitoriesAsync_StudentGetsNone);
        using var context = CreateDbContext(dbName);
        await SeedDormitoriesAndManagersAsync(context);

        var service = new RoomService(context);

        // Act
        var result = await service.GetAllowedDormitoriesAsync(
            userId: 999,
            role: UserRole.Student.ToString());

        // Assert
        Assert.Empty(result);
    }

    // ---------- AddRoomAsync ----------

    [Fact]
    public async Task AddRoomAsync_AdminCanAddRoomToAnyDormitory()
    {
        // Arrange
        var dbName = nameof(AddRoomAsync_AdminCanAddRoomToAnyDormitory);
        using var context = CreateDbContext(dbName);
        await SeedDormitoriesAndManagersAsync(context);

        var service = new RoomService(context);

        var input = new CreateRoomInputModel
        {
            DormitoryId = 2,  // West Residence (not managed by manager)
            RoomNumber = 201,
            Floor = 2,
            WingOrBlock = 2,
            Capacity = 2,
            HasPrivateBathroom = false,
            GenderType = GenderType.Female,
            IsActive = true
        };

        // Act
        var room = await service.AddRoomAsync(
            input,
            userId: 1,                      // some admin id
            role: UserRole.Admin.ToString());

        // Assert
        Assert.NotNull(room);
        Assert.Equal(2, room.DormitoryId);
        Assert.Equal(201, room.RoomNumber);

        var roomsInDb = await context.Rooms.ToListAsync();
        Assert.Single(roomsInDb);
    }

    [Fact]
    public async Task AddRoomAsync_ManagerCanAddRoomToOwnDormitory()
    {
        // Arrange
        var dbName = nameof(AddRoomAsync_ManagerCanAddRoomToOwnDormitory);
        using var context = CreateDbContext(dbName);
        await SeedDormitoriesAndManagersAsync(context);

        var service = new RoomService(context);

        var input = new CreateRoomInputModel
        {
            DormitoryId = 1,  // North Hall (managed by user 2)
            RoomNumber = 101,
            Floor = 1,
            WingOrBlock = 1,
            Capacity = 3,
            HasPrivateBathroom = true,
            GenderType = GenderType.Male,
            IsActive = true
        };

        // Act
        var room = await service.AddRoomAsync(
            input,
            userId: 2,                                   // dormmanager (from seed)
            role: UserRole.DormitoryManager.ToString());

        // Assert
        Assert.NotNull(room);
        Assert.Equal(1, room.DormitoryId);
        Assert.Equal(101, room.RoomNumber);

        var roomsInDb = await context.Rooms.ToListAsync();
        Assert.Single(roomsInDb);
    }

    [Fact]
    public async Task AddRoomAsync_NonAdminNonManagerThrowsUnauthorized()
    {
        // Arrange
        var dbName = nameof(AddRoomAsync_NonAdminNonManagerThrowsUnauthorized);
        using var context = CreateDbContext(dbName);
        await SeedDormitoriesAndManagersAsync(context);

        var service = new RoomService(context);

        var input = new CreateRoomInputModel
        {
            DormitoryId = 1,
            RoomNumber = 401,
            Floor = 4,
            WingOrBlock = 4,
            Capacity = 1,
            HasPrivateBathroom = false,
            GenderType = GenderType.Male,
            IsActive = true
        };

        // Act + Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.AddRoomAsync(
                input,
                userId: 999,
                role: UserRole.Student.ToString()));

        Assert.Empty(context.Rooms);
    }

    [Fact]
    public async Task AddRoomAsync_ThrowsIfDormitoryNotFound()
    {
        // Arrange
        var dbName = nameof(AddRoomAsync_ThrowsIfDormitoryNotFound);
        using var context = CreateDbContext(dbName);
        // note: not seeding dorms here

        var service = new RoomService(context);

        var input = new CreateRoomInputModel
        {
            DormitoryId = 999, // non-existent
            RoomNumber = 1,
            Floor = 1,
            Capacity = 2,
            GenderType = GenderType.Male,
            IsActive = true
        };

        // Act + Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.AddRoomAsync(
                input,
                userId: 1,
                role: UserRole.Admin.ToString()));

        Assert.Empty(context.Rooms);
    }
}