using Microsoft.IdentityModel.Tokens;
using PMS.Infrastructre.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Infrastructre.Interfaces
{
    public interface ITokenService
    {
        Task<SecurityToken> GenerateJwtAsync(AppUser user);
      //  Task<RefreshTokenModel> CreateRefreshTokenAsync(AppUser user, string? refreshToken = null);
        void RevokeAllAsync(AppUser user);
    }
}
