using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartDorm.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace SmartDorm.Pages.Users.Login;


public class LoginModel : PageModel
{
    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public LoginInputModel Input { get; set; } = new();

    public string? ReturnUrl { get; set; }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? "/";
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl ??= returnUrl ?? "/";

        if (!ModelState.IsValid)
            return Page();

        var user = await _context.UserAccounts
            .FirstOrDefaultAsync(u => u.Username == Input.Username && u.IsActive);

        if (user == null || user.PasswordHash != HashPassword(Input.Password))
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return Page();
        }

        // build claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = Input.RememberMe
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        if (Url.IsLocalUrl(ReturnUrl))
            return Redirect(ReturnUrl);

        return RedirectToPage("/Index");
    }

    // same hashing as register (todo: create service for that)
    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
