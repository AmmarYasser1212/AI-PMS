using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Auth
{
    public class AuthModel
    {

        public string Message { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public bool IsAuthenticated { get; set; }

        public List<string>Roles { get; set; }

        public DateTime ExpiresOn { get; set; }

       
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }


    }
}
