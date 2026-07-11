using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Message { get; set; }=string.Empty;
        public string Type { get; set; }   // REMINDER / AI / SYSTEM
        public string Source { get; set; } // TASK / SCHEDULE / AI
        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
