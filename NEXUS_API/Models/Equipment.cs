﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }       
        public bool Status { get; set; }
        public int TypeId { get; set; }
        public EquipmentType? EquipmentType { get; set; }
        public string? DiscountId { get; set; }
        public Discount? Discount { get; set; }
    }
}