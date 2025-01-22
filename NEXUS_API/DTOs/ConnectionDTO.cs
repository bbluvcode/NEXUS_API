namespace NEXUS_API.DTOs
{
    public class ConnectionDTO
    {
        public string OrderId { get; set; }
        public int EquipmentId { get; set; } 
        public int PlanId { get; set; }
        public int NumberOfConnections { get; set; }
    }
}