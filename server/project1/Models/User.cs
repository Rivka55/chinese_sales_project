using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project1.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [MaxLength(50, ErrorMessage = "שם לא יכול להכיל יותר מ-50 תווים")]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "שדה חובה")]
        [Phone(ErrorMessage = "מספר טלפון לא תקין")]
        public string Phone { get; set; } = string.Empty;


        [Required(ErrorMessage = "שדה חובה")]
        [EmailAddress(ErrorMessage = "כתובת אימייל לא תקינה")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "שדה חובה")]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;


        [Required(ErrorMessage = "שדה חובה")]
        public string Role { get; set; } = "user";


        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
