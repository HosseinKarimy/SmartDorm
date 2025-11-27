using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartDorm.Data;
using SmartDorm.Models;
using Microsoft.EntityFrameworkCore;
using SmartDorm.Services;

namespace SmartDorm.Pages.Features.Dormitories.RoomManagement.CreateRoom;

[Authorize(Roles = "Admin,DormitoryManager")]
public class CreateRoomModel(IRoomService _roomService) : PageModel
{
    public List<Dormitory> Dormitories { get; set; } = new();

    [BindProperty]
    public CreateRoomInputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var (userId, role) = GetCurrentUser();
        Dormitories = await _roomService.GetAllowedDormitoriesAsync(userId, role);

        if (!Dormitories.Any())
        {
            return Unauthorized();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var (userId, role) = GetCurrentUser();
        Dormitories = await _roomService.GetAllowedDormitoriesAsync(userId, role);

        if (!ModelState.IsValid)
        {
            // Need dorm list to re-render form after validation errors
            return Page();
        }

        try
        {
            await _roomService.AddRoomAsync(Input, userId, role);
        }
        catch (UnauthorizedAccessException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }

        return RedirectToPage("/Rooms/Index");
    }

    private (int userId, string role) GetCurrentUser()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("User id claim missing.");

        var role = User.FindFirst(ClaimTypes.Role)?.Value
            ?? throw new InvalidOperationException("Role claim missing.");

        return (int.Parse(idClaim), role);
    }
}
