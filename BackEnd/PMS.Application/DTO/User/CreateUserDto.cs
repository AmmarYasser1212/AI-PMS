using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.User
{
    public class CreateUserDto
    {
        // Email is required and must be a valid email format
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public string Email { get; set; } = null!;

        // Password is required and must be strong (min 6 characters)
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(100, ErrorMessage = "Password must not exceed 100 characters.")]
        public string Password { get; set; } = null!;

        // Name is required and must be between 3 and 50 characters
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters.")]
        public string Name { get; set; } = null!;

        // Avatar is optional but must not exceed 255 characters
        [MaxLength(255, ErrorMessage = "Avatar URL must not exceed 255 characters.")]
        public string? Avatar { get; set; }
    }
}
