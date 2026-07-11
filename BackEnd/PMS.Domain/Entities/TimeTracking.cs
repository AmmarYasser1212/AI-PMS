using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Entities
{
    public class TimeTracking
    {


        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }    
        public DateTime? EndedAt { get; set; }        

        public int AccumulatedSeconds { get; set; }  
        public bool IsPaused { get; set; }
   
      
        public int CurrentDuration => IsPaused
            ? AccumulatedSeconds
            : AccumulatedSeconds + (int)(DateTime.UtcNow - StartedAt).TotalSeconds;

        public TaskItem Task { get; set; }
    }
}
