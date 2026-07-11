using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces.Services
{
    public interface IEmailServices
    {
        Task EmailSendAsync(string toEmail,string subject,string body);
    }
}
