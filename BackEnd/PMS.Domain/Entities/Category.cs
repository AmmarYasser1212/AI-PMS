using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
   public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; } 

        public ICollection<TaskItem> Tasks { get; set; } = [];
    }
}
