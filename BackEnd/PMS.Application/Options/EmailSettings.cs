using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Options
{
    public class EmailSettings
    {
        public string SmtpServer {  get; set; }=string.Empty;
        public string SenderName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string AppPassword { get; set; } = string.Empty;

        public int Port { get; set; }

    }
}
