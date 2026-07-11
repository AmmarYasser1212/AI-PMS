using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.User
{
    public class UpdateUserDto
    {
        //// Id is required to identify the user
        //[Required(ErrorMessage = "User ID is required.")]
        //[Range(1, int.MaxValue, ErrorMessage = "Invalid User ID.")]
        //public int Id { get; set; }

        // Name is required and must be between 3 and 50 characters
       
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "Name must not exceed 50 characters.")]
        public string? Name { get; set; } 

        // Avatar is optional
        [MaxLength(255, ErrorMessage = "Avatar URL must not exceed 255 characters.")]
        public string? Avatar { get; set; }
    }
}
