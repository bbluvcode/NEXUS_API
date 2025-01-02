using System.ComponentModel.DataAnnotations;

public class FeedBack
{
    [Key]
    public int FeedBackId { get; set; }

    [Required(ErrorMessage = "Customer ID is required")]
    [StringLength(50, ErrorMessage = "Customer ID cannot be longer than 50 characters")]
    public string CustomerId { get; set; } // Foreign Key to Customer

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(50, ErrorMessage = "Email cannot be longer than 50 characters")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Feedback content is required")]
    [StringLength(1000, ErrorMessage = "Content cannot be longer than 1000 characters")]
    public string FBContent { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public bool Status { get; set; } // Indicates whether feedback has been read
}
