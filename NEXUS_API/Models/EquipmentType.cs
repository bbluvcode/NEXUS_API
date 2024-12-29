using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class EquipmentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }
        [Required]
        public string TypeName { get; set; }
        [Required]
        public string Provider { get; set; }
        public ICollection<Equipment>? Equipments { get; set; }
    }
}
