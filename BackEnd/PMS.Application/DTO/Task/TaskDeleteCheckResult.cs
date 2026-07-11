using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Task
{
    public class TaskDeleteCheckResult
    {
        public bool CanDeleteDirectly { get; set; }

        public bool HasScheduleConflict { get; set; }

        public string? Message { get; set; }

        public List<string>? Options { get; set; }
    }
}
