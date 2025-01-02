using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NEXUS_API.Models
{
    public class RetainShop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RetainShopId { get; set; }
        [Required]
        [StringLength(15)]
        public string RetainShopName { get; set; }
        [Required]
        [StringLength(50)]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(10)]
        public string Phone { get; set; }
        public bool IsMainOffice { get; set; }
        [Required]
        [StringLength(15)]
        public string Fax { get; set; }

    }
}
