using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.DTOs
{
    public class UpdateSurveyStatusDTO
    {
        public string OrderId { get; set; }
        public int SurveyorId { get; set; }
        public string SurveyStatus { get; set; } 
        public string? SurveyDescribe { get; set; }
    }
}
