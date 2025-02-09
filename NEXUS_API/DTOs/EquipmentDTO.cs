namespace NEXUS_API.DTOs
{
    public class EquipmentDTO
    {
        public string EquipmentName { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string? DiscountId { get; set; }
        public int EquipmentTypeId { get; set; }
        public int VendorId { get; set; }
        public int StockId { get; set; }
    }
}
