﻿using System.ComponentModel.DataAnnotations;

public class News
{
    [Key]
    public int NewsID { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Employee ID is required")]
    [StringLength(50, ErrorMessage = "Employee ID cannot be longer than 50 characters")]
    public string EmpID { get; set; } // Foreign Key to Employee

    [Required(ErrorMessage = "Content is required")]
    [StringLength(1000, ErrorMessage = "Content cannot be longer than 1000 characters")]
    public string Content { get; set; }

    [Required(ErrorMessage = "Creation date is required")]
    public DateTime CreateDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public bool Status { get; set; } // News status
}