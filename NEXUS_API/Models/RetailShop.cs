using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NEXUS_API.Models
{
    public class RetailShop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RetailShopId { get; set; }
        [Required]
        [StringLength(15)]
        public string RetailShopName { get; set; }
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

        //Relationship
        [Required]
        public int RegionId { get; set; }
        public Region? Region { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
