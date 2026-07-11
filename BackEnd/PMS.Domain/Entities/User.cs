using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class User 
    {
        public int Id { get; set; } 

         public string Email { get; set; } = null!;

        public string IdentityUserId { get; set; } = null!;

        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }

        public DateTime CreatedAt { get; set; }

        public NotificationSettings NotificationSettings { get; set; } = new NotificationSettings();

        public ICollection<TaskItem>? Tasks { get; set; } = new List<TaskItem>();
        public ICollection<Category>? Categories { get; set; } = new List<Category>();
        public ICollection<Tag>? Tags { get; set; } = new List<Tag>();

        public ICollection<Notification>? Notifications { get; set; } = new List<Notification>();

        public ICollection<Schedule>? Schedules { get; set; } = new List<Schedule>();

        public ICollection<AiReport>? AiReports { get; set; } = new List<AiReport>();
        public ICollection<UserProductivityStat>? UserProductivityStats { get; set; } = new List<UserProductivityStat>();
    }

}
