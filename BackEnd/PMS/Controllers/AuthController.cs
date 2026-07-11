using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PMS.Application.DTO.Auth;
using PMS.Application.DTO.External;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using PMS.Helpers;
using PMS.Infrastructre.Services.AuthService;
using System.Net;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IAuthService _authService;
        private readonly IEmailServices _emailServices;


        public AuthController(IAuthService authService, IGoogleAuthService googleAuthService,IEmailServices emailServices)
        {
            _authService = authService;
            _googleAuthService = googleAuthService;
            _emailServices = emailServices;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            SetTokenInCookie(result.Token, result.ExpiresOn);//
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);//


            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            SetTokenInCookie(result.Token,result.ExpiresOn /*DateTime.UtcNow.AddDays(7)*/);

            return Ok();
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);


            SetTokenInCookie(result.Token, result.ExpiresOn /*DateTime.UtcNow.AddDays(7)*/);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);


            return Ok();
        }
        [Authorize(Roles = "User")]
        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken(/*[FromBody] RevokeToken model*/)
        {
            var userId = User.GetBusinessUserId();

            var result = await _authService.RevokeTokenAsync(userId);

            if (!result)
                return BadRequest("Token is invalid!");

            // 🧹 clear cookies
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("refreshToken");

            return Ok("Logged out successfully");
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            if (expires <= DateTime.UtcNow)
                expires = DateTime.UtcNow.AddDays(7);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
             private void SetTokenInCookie(string token, DateTime expires)
        {
            if (expires <= DateTime.UtcNow)
                expires = DateTime.UtcNow.AddMinutes(15);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("Token",token, cookieOptions);
        }


        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var result = await _googleAuthService.LoginWithGoogleAsync(request.IdToken);

            if (!result.IsAuthenticated)
                return BadRequest("Invalid Google token");

            SetTokenInCookie(result.Token, result.ExpiresOn);//
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);//

            return Ok(result);
        }

        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(RequestForgotPasswordDto request )
        {

            var result = await _authService.ForgotPassword(request);

            if(!string.IsNullOrWhiteSpace(result.errorMessage))
            {
                return BadRequest(result.errorMessage);
            }
            var encodedToken = WebUtility.UrlEncode(result.token);

            var resetlink = Url.Action("ChangePassword", "Auth", new { email = result.email, token = result.token },Request.Scheme);

           // var callbackUrl = $"http://localhost:1000/restpass?code={result.token}&email={result.email}";

            var subject = "Reset Password";

            var body = $"Please reset your password by clicking here: <a href='{resetlink}'>Reset Password</a> ";

            await _emailServices.EmailSendAsync(request.Email, subject, body);

            return Ok("open your gmail");

        }

        [Route("ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordReqDto reqreset)
        {
            reqreset.Token = WebUtility.UrlDecode(reqreset.Token);
            var result = await _authService.ResetPassword(reqreset);

            if(result== "password restet is successful") {return Ok(result); }

            return BadRequest(result);
        }


        }


    }
    

