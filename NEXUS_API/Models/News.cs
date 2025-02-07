using System;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Employee ID is required")]
        public int EmployeeId { get; set; } // Foreign Key

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } // Lưu HTML có ảnh

        [Required(ErrorMessage = "Creation date is required")]
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public bool Status { get; set; }

        // Relationship
        public Employee? Employee { get; set; }
    }
}
