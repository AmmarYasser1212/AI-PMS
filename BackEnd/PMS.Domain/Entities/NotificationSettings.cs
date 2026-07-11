using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class NotificationSettings
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public bool EnableMotivation { get; set; }
        public bool EnableReminders { get; set; }
        public bool EnableAiTips { get; set; }

        public TimeOnly? PreferredTime { get; set; }
    }
}
