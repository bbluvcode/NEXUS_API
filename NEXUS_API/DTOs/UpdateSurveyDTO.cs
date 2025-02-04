using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class UpdateSurveyDTO
    {
        public string SurveyStatus { get; set; }
        public int? PlanFeeId { get; set; } 
        public int? NumberOfConnections { get; set; }
        public int? EquipmentId { get; set; }
        public string? CancellationReason { get; set; }
    }
}
