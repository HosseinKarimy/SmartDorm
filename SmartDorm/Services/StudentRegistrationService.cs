using SmartDorm.Data;
using SmartDorm.Enums;
using System.Text;
using SmartDorm.Models;
using SmartDorm.Pages.Features.StudentProfileManagement.RegisterStudent;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace SmartDorm.Services;

public class RegistrationResult
{
    public bool Succeeded { get; set; }
    public string? ErrorMessage { get; set; }
    public UserAccount? User { get; set; }
    public Student? Student { get; set; }

    public static RegistrationResult Success(UserAccount u, Student s) =>
        new() { Succeeded = true, User = u, Student = s };

    public static RegistrationResult Fail(string error) =>
        new() { Succeeded = false, ErrorMessage = error };
}

public interface IStudentRegistrationService
{
    Task<RegistrationResult> RegisterStudentAsync(RegisterInputModel input);
}

public class StudentRegistrationService(AppDbContext context) : IStudentRegistrationService
{
    public async Task<RegistrationResult> RegisterStudentAsync(RegisterInputModel input)
    {
        // username uniqueness
        var exists = await context.UserAccounts
            .AnyAsync(u => u.Username == input.Username);

        if (exists)
            return RegistrationResult.Fail("Username is already taken.");

        var user = new UserAccount
        {
            Username = input.Username,
            PasswordHash = HashPassword(input.Password),
            Role = UserRole.Student,
            IsActive = true
        };

        context.UserAccounts.Add(user);
        await context.SaveChangesAsync();

        var student = new Student
        {
            StudentNumber = input.StudentNumber,
            FirstName = input.FirstName,
            LastName = input.LastName,
            Gender = input.Gender,
            BirthdayYear = input.BirthdayYear,
            FieldOfStudy = input.FieldOfStudy,
            EntryYear = input.EntryYear,
            DegreeLevel = input.DegreeLevel,
            Email = input.Email,
            PhoneNumber = input.PhoneNumber,
            UserId = user.Id
        };

        context.Students.Add(student);
        await context.SaveChangesAsync();

        var pref = new StudentPreference
        {
            StudentId = student.StudentId,
            SleepHabit = input.SleepHabit,
            NoiseToleranceLevel = input.NoiseToleranceLevel,
            StudyStyle = input.StudyStyle,
            RoomAtmosphere = input.RoomAtmosphere,
            RoomCapacity = input.RoomCapacity
        };
        context.StudentPreferences.Add(pref);

        if (input.HasSpecialCondition)
        {
            var cond = new SpecialCondition
            {
                StudentId = student.StudentId,
                ConditionType = input.ConditionType,
                Description = input.ConditionDescription,
                Severity = input.ConditionSeverity,
                Needs = input.ConditionNeeds
            };
            context.SpecialConditions.Add(cond);
        }

        await context.SaveChangesAsync();

        return RegistrationResult.Success(user, student);
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(sha.ComputeHash(bytes));
    }
}
