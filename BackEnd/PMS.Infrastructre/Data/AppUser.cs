using Microsoft.AspNetCore.Identity;
using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Infrastructre.Data
{
    public class AppUser :IdentityUser
    {
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
