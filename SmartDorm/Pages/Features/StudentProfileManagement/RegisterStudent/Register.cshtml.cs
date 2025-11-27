using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartDorm.Data;
using SmartDorm.Enums;
using SmartDorm.Models;

namespace SmartDorm.Pages.Features.StudentProfileManagement.RegisterStudent
{
    public class RegisterModel(AppDbContext context) : PageModel
    {
        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public void OnGet()
        {
            // Could set defaults here if needed
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // check username uniqueness
            var existingUser = await context.UserAccounts
                .AnyAsync(u => u.Username == Input.Username);

            if (existingUser)
            {
                ModelState.AddModelError("Input.Username", "This username is already taken.");
                return Page();
            }

            // Create UserAccount
            var user = new UserAccount
            {
                Username = Input.Username,
                PasswordHash = HashPassword(Input.Password),
                Role = UserRole.Student,
                IsActive = true
            };

            context.UserAccounts.Add(user);
            await context.SaveChangesAsync(); // ensure user.Id is generated

            // Create Student
            var student = new Student
            {
                StudentNumber = Input.StudentNumber,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                BirthdayYear = Input.BirthdayYear,
                FieldOfStudy = Input.FieldOfStudy,
                EntryYear = Input.EntryYear,
                DegreeLevel = Input.DegreeLevel,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                UserId = user.Id
            };

            context.Students.Add(student);
            await context.SaveChangesAsync(); // get StudentId

            // Create StudentPreference
            var pref = new StudentPreference
            {
                StudentId = student.StudentId,
                SleepHabit = Input.SleepHabit,
                NoiseToleranceLevel = Input.NoiseToleranceLevel,
                StudyStyle = Input.StudyStyle,
                RoomAtmosphere = Input.RoomAtmosphere,
                RoomCapacity = Input.RoomCapacity
            };

            context.StudentPreferences.Add(pref);

            // Optional SpecialCondition
            if (Input.HasSpecialCondition)
            {
                var cond = new SpecialCondition
                {
                    StudentId = student.StudentId,
                    ConditionType = Input.ConditionType,
                    Description = Input.ConditionDescription,
                    Severity = Input.ConditionSeverity,
                    Needs = Input.ConditionNeeds
                };

                context.SpecialConditions.Add(cond);
            }

            await context.SaveChangesAsync();

            return RedirectToPage("/SuccessPage");
        }

        // Simple hash for demo.
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

}

