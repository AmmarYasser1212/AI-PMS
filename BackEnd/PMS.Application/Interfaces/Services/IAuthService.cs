using PMS.Application.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterDto dto);
        Task<AuthModel> LoginAsync(LoginDto dto);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(int UserId);
        public Task<responseforgetpass> ForgotPassword(RequestForgotPasswordDto req);
        public Task<string> ResetPassword(ResetPasswordReqDto req);
    }
}
