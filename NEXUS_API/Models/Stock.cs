using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class Stock
    {
        [Key]
        public int StockId { get; set; } 

        [Required]
        [MaxLength(100)]
        public string StockName { get; set; } 

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } 

        [Required]
        [MaxLength(50)]
        public string Email { get; set; } 

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } 

        [Required]
        [MaxLength(30)]
        public string Fax { get; set; }

        //Relationship
        [Required]
        public int RetainShopId { get; set; }
        public RetainShop? RetainShop { get; set; }
        public ICollection<Equipment>? Equipments { get; set; }
        public ICollection<InStockOrder>? InStockOrders { get; set; }
        public ICollection<OutStockOrder>? OutStockOrders { get; set; }
    }
}
