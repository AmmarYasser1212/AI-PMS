using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Task
{
    public class UpdateTaskDto
    {
      

        // ✏️ Editable fields
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        // ⏱ Core AI fields
        [Range(1, int.MaxValue)]
        public int? DurationInMinutes { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime? EarliestStart { get; set; }

        public DateTime? LatestEnd { get; set; }

        // 🎯 AI ranking
        [Range(1, 10)]
        public int? Priority { get; set; }

        [Range(1, 5)]
        public int? EffortLevel { get; set; }

        // 📊 Status
        public TaskStatus? Status { get; set; }

        // 📁 Category
        public int? CategoryId { get; set; }
    }
}
