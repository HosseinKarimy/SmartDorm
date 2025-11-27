using Microsoft.EntityFrameworkCore;
using SmartDorm.Data;
using SmartDorm.Enums;
using SmartDorm.Models;
using SmartDorm.Pages.Features.StudentProfileManagement.RegisterStudent;
using SmartDorm.Services;

namespace SmartDormTests;

public class StudentRegistrationServiceTests
{
    private AppDbContext CreateDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
        .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task RegisterStudentAsync_creates_user_student_and_preference()
    {
        // Arrange
        var context = CreateDbContext(nameof(RegisterStudentAsync_creates_user_student_and_preference));
        var service = new StudentRegistrationService(context);

        var input = new RegisterInputModel
        {
            Username = "student1",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            StudentNumber = "S123",
            FirstName = "John",
            LastName = "Doe",
            Gender = GenderType.Male,
            EntryYear = 2024,
            DegreeLevel = DegreeLevel.Bachelor,
            SleepHabit = SleepHabit.EarlyBird,
            NoiseToleranceLevel = NoiseToleranceLevel.Moderate,
            StudyStyle = StudyStyle.Individual,
            RoomAtmosphere = RoomAtmosphere.Quiet,
            RoomCapacity = 2,
            HasSpecialCondition = false
        };

        // Act
        var result = await service.RegisterStudentAsync(input);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.User);
        Assert.NotNull(result.Student);

        Assert.Equal(1, await context.UserAccounts.CountAsync());
        Assert.Equal(1, await context.Students.CountAsync());
        Assert.Equal(1, await context.StudentPreferences.CountAsync());
        Assert.Equal(0, await context.SpecialConditions.CountAsync());
    }

    [Fact]
    public async Task RegisterStudentAsync_fails_when_username_taken()
    {
        // Arrange
        var context = CreateDbContext(nameof(RegisterStudentAsync_fails_when_username_taken));
        context.UserAccounts.Add(new UserAccount
        {
            Username = "student1",
            PasswordHash = "xxx",
            Role = UserRole.Student,
            IsActive = true
        });
        await context.SaveChangesAsync();

        var service = new StudentRegistrationService(context);

        var input = new RegisterInputModel
        {
            Username = "student1",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            StudentNumber = "S124",
            FirstName = "Other",
            LastName = "User",
            Gender = GenderType.Female,
            EntryYear = 2024,
            DegreeLevel = DegreeLevel.Bachelor,
            SleepHabit = SleepHabit.Flexible,
            NoiseToleranceLevel = NoiseToleranceLevel.Quiet,
            StudyStyle = StudyStyle.Group,
            RoomAtmosphere = RoomAtmosphere.Social,
            RoomCapacity = 3
        };

        // Act
        var result = await service.RegisterStudentAsync(input);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Equal("Username is already taken.", result.ErrorMessage);
    }
}