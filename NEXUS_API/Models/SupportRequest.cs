using System.ComponentModel.DataAnnotations;

public class SupportRequest
{
    [Key]
    public int SupportRequestId { get; set; }

    [Required(ErrorMessage = "Customer ID is required")]
    [StringLength(50, ErrorMessage = "Customer ID cannot be longer than 50 characters")]
    public int CustomerID { get; set; } // Foreign Key to Customer

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    [StringLength(50, ErrorMessage = "Phone number cannot be longer than 50 characters")]
    public string PhoneNo { get; set; }

    [Required(ErrorMessage = "Date of request is required")]
    public DateTime DateRequest { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Detail content is required")]
    [StringLength(1000, ErrorMessage = "Content cannot be longer than 1000 characters")]
    public string DetailContent { get; set; }
}
