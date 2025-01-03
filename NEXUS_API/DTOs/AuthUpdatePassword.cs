using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class AuthUpdatePassword
    {
        public int UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
