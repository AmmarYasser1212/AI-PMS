using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class UserProductivityStat
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }

        public int TotalTime { get; set; }
        public int CompletedTasks { get; set; }
        public int CreatedTasks { get; set; }

        public float ProductivityScore { get; set; }
    }
}
