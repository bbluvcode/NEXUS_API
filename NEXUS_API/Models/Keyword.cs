using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class Keyword
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Words { get; set; }
        public bool Status { get; set; }
    }
}
