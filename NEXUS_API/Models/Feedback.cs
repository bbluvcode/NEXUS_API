﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEXUS_API.Models
{
    public class FeedBack
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedBackId { get; set; }
        [Required]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Feedback content is required")]
        [StringLength(1000, ErrorMessage = "Content cannot be longer than 1000 characters")]
        public string FeedBackContent { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public bool Status { get; set; } // Indicates whether feedback has been read

        //Relationship
        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
