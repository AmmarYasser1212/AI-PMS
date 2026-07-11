using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Category
{
    public class UpdateCategory
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
