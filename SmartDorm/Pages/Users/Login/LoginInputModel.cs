using System.ComponentModel.DataAnnotations;

namespace SmartDorm.Pages.Users.Login
{
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
