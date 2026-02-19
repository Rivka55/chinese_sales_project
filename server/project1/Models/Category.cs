using System.ComponentModel.DataAnnotations;

namespace project1.Models
{
    public class Category
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "שדה חובה")]
        [MaxLength(50, ErrorMessage = "שם הקטגוריה לא יכול להכיל יותר מ-50 תווים")]
        public string Name { get; set; } = string.Empty;
    }
}