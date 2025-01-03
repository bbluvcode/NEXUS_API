using NEXUS_API.Models;
using System.ComponentModel.DataAnnotations;

public class CustomerRequest
{
    [Key]
    public int RequestId { get; set; }

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

    //Relationship
    [Required]
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
}
