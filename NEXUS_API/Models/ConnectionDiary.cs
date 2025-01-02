using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class ConnectionDiary
    {
        [Key]
        public int DiaryId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        //Relationship
        [Required]
        public int ConnectionId { get; set; }
        public Connection? Connection { get; set; }
    }
}
