using System;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class Region
    {
        [Key]
        public int RegionId { get; set; } // Primary key, Region ID

        [Required(ErrorMessage = "Region code is required.")]
        [StringLength(20, ErrorMessage = "Region code cannot exceed 20 characters.")]
        public string RegionCode { get; set; } // Region code (varchar(20))

        [Required(ErrorMessage = "Region name is required.")]
        [StringLength(20, ErrorMessage = "Region name cannot exceed 20 characters.")]
        public string RegionName { get; set; } // Name of region (varchar(20))
    }
}
