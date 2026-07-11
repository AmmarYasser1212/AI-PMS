using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class Schedule
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Type { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public TimeSpan StartWorkingHour { get; set; }
        public TimeSpan EndWorkingHour { get; set; }

        public bool IsActive { get; set; }=true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ScheduleTask> ScheduleTasks { get; set; }
    }
}
