using System.ComponentModel.DataAnnotations;

namespace project1.DTOs.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "חובה להזין אימייל")]
        [EmailAddress(ErrorMessage = "אימייל לא תקין")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "חובה להזין סיסמה")]
        public string Password { get; set; } = string.Empty;
    }
}