using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Task
{
    public class DeleteResolutionRequest
    {
        public string Option { get; set; }  // ReplaceTask, ReplanSchedule...
        public int? NewTaskId { get; set; }  // needed only for ReplaceTask
    }
}
