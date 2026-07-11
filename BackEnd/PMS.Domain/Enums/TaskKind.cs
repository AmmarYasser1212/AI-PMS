using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Domain.Enums
{

        public enum TaskKind
        {
            Task=0,   // AI can schedule it
            Event=1   // Fixed time block (meeting, doctor, etc.)
        }
}
