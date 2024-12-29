using System.ComponentModel.DataAnnotations;

public class CustomerRequest
{
    [Key]
    public int RequestID { get; set; }

    [Required]
    [StringLength(15, ErrorMessage = "Customer ID must not exceed 15 characters.")]
    public string CusID { get; set; } // Foreign Key to Customer table

    [Required]
    [StringLength(20, ErrorMessage = "Request Title must not exceed 20 characters.")]
    public string RequestTitle { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "Service Request must not exceed 1000 characters.")]
    public string ServiceRequest { get; set; }

    [StringLength(1000, ErrorMessage = "Equipment Request must not exceed 1000 characters.")]
    public string EquipmentRequest { get; set; }

    [Required]
    public bool IsResponse { get; set; } // Indicates if the request has been responded to
}
