using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Category
{
    public class CategoryDeleteCheckResult
    {
        public bool CanDeleteDirectly { get; set; }
        public bool HasScheduledTasks { get; set; }
        public string? Message { get; set; }
        public List<string>? Options { get; set; }
    }
}
