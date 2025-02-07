using System.ComponentModel.DataAnnotations;

public class NewsCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Employee ID is required")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public bool Status { get; set; }

    public IFormFile? ImageFile { get; set; } // Ảnh không bắt buộc
}
