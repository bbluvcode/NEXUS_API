using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class ConnectionDiary
    {
        [Key]
        public int DiaryID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string ConnID { get; set; }
    }
}
