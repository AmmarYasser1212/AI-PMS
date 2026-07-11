using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Tag
{
    public class CreateTagDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        //public int UserId { get; set; }
    }
}
