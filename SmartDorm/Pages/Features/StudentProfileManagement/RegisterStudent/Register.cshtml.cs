using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartDorm.Services;

namespace SmartDorm.Pages.Features.StudentProfileManagement.RegisterStudent
{
    public class RegisterModel(IStudentRegistrationService _registration) : PageModel
    {
        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _registration.RegisterStudentAsync(Input);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage!);
                return Page();
            }

            return RedirectToPage("/Features/StudentProfileManagement/RegisterStudent/SuccessPage");
        }
    }

}

