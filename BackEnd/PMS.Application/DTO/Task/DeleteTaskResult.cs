using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Task
{
    public class DeleteTaskResult
    {
        public bool Success { get; set; }

        public bool NotFound { get; set; }

        public bool HasScheduleConflict { get; set; }

        public List<string>? Options { get; set; }

        public string? Message { get; set; }
    }
}
