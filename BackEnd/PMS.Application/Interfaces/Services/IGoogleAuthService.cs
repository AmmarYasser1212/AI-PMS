using PMS.Application.DTO.Auth;
using PMS.Application.DTO.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces.Services
{
    public interface IGoogleAuthService
    {
        Task<AuthModel> LoginWithGoogleAsync(string idToken);
    }
}
