using System;
using System.ComponentModel.DataAnnotations;

namespace NEXUS_API.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; } // Primary key
        [Required]
        public int RetainShopId { get; set; } //retain shop ID
        [Required(ErrorMessage = "Employee code is required.")]
        [StringLength(5, ErrorMessage = "Employee code cannot exceed 5 characters.")]
        public string EmployeeCode { get; set; } // Employee token (varchar(5))

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(50, ErrorMessage = "Full name cannot exceed 50 characters.")]
        public string FullName { get; set; } // Employee’s full name (varchar(50))

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(7, ErrorMessage = "Gender cannot exceed 7 characters.")]
        public string Gender { get; set; } // Employee’s gender (varchar(7))

        [Required(ErrorMessage = "Birthdate is required.")]
        public DateTime Birthdate { get; set; } // Employee’s birthdate

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Address { get; set; } // Employee’s address (varchar(200))

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
        public string Email { get; set; } // Employee’s email (varchar(50))

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string Phone { get; set; } // Employee’s phone number (varchar(20))

        [Required(ErrorMessage = "Identification number is required.")]
        [StringLength(20, ErrorMessage = "Identification number cannot exceed 20 characters.")]
        public string IdentificationNo { get; set; } // Identification number of Employee (varchar(20))

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, ErrorMessage = "Password cannot exceed 20 characters.")]
        public string Password { get; set; } // Password to login system (varchar(20))

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(20, ErrorMessage = "Role cannot exceed 20 characters.")]
        public string Role { get; set; } // Role of employee (varchar(20))

        [Required(ErrorMessage = "Status is required.")]
        public bool Status { get; set; } // Status work of employee (bool)

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpried { get; set; }
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? ExpiredBan { get; set; }
        public string? Code { get; set; }
        public DateTime? ExpiredCode { get; set; }
        public int SendCodeAttempts { get; set; } = 0;
        public DateTime? LastSendCodeDate { get; set; }
    }
}
