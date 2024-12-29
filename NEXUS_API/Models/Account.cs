using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models.Man
{
    public class Account
    {
        [Key]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Account ID must be exactly 16 characters.")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Account ID must contain only digits.")]
        public string AccID { get; set; }

        [Required]
        public int CusID { get; set; } // Foreign Key to Customer table

        [Required]
        public int DiscountID { get; set; } // Foreign Key to Discount table        
    }
}
