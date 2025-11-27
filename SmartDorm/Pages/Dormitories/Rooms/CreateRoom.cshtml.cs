using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartDorm.Data;
using SmartDorm.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartDorm.Pages.Dormitories.Rooms;

[Authorize(Roles = "Admin,DormitoryManager")]
public class CreateRoomModel(AppDbContext context) : PageModel
{

    // For dropdown
    public List<Dormitory> Dormitories { get; set; } = new();

    [BindProperty]
    public Room Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        // Admin sees all dormitories
        // Dorm managers see only their assigned one(s)
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (role == "Admin")
        {
            Dormitories = await context.Dormitories.ToListAsync();
        }
        else // DormitoryManager
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            // find manager's dormitory
            var managerDormId = await context.DormitoryManagers
                .Where(dm => dm.UserId == userId)
                .Select(dm => dm.Id)
                .FirstOrDefaultAsync();

            Dormitories = await context.Dormitories
                .Where(d => d.DormitoryId == managerDormId)
                .ToListAsync();
        }

        if (!Dormitories.Any())
            return Unauthorized(); // no dormitory assigned

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Validation
        if (!ModelState.IsValid)
        {
            await OnGetAsync(); // re-load dormitory list
            return Page();
        }

        // Security: ensure user can add to this dormitory
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (role == "DormitoryManager")
        {
            bool ownsDorm = await context.DormitoryManagers
                .AnyAsync(dm => dm.UserId == userId && dm.Id == Input.DormitoryId);

            if (!ownsDorm)
                return Unauthorized();
        }

        // Save room
        context.Rooms.Add(Input);
        await context.SaveChangesAsync();

        return RedirectToPage("/Rooms/Index");
    }
}
