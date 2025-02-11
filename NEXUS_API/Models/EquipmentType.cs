﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class EquipmentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EquipmentTypeId { get; set; }
        [Required]
        public string TypeName { get; set; }
        [Required]
        public string Provider { get; set; }

        //Relationship
        public ICollection<Equipment>? Equipments { get; set; }
    }
}
