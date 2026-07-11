using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMS.Application.DTO.Auth;
using PMS.Application.Interfaces.Repositories;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using PMS.Infrastructre.Data;
using PMS.Infrastructre.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace PMS.Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly Irepsitory<User> _user;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IunitOfWork _uow;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, IunitOfWork uow,
            Irepsitory<User> user,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _uow = uow;
            _user = user;
            _tokenService = tokenService;
        }
        public async Task<AuthModel> LoginAsync(LoginDto request)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            //if (!user.EmailConfirmed)
            //{
            //    authModel.Message = "Email  was not Confirmed!";
            //    return authModel;
            //}

            var domainUser = await _user.FindOneAsync(u => u.IdentityUserId == user.Id)
        ?? throw new Exception(nameof(User));

            var jwtToken = await _tokenService.GenerateJwtAsync(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpiresOn = jwtToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return authModel;
        }


        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }


        public async Task<AuthModel> RegisterAsync(RegisterDto request)
        {
            AppUser? identityUser = null;
            User? businessUser = null;
            try
            {
                if (await _userManager.FindByEmailAsync(request.Email) is not null)
                    return new AuthModel { Message = "Email is already registered!" };

                if (await _userManager.FindByNameAsync(request.UserName) is not null)
                    return new AuthModel { Message = "Username is already registered!" };

                identityUser = new AppUser
                {
                    UserName = request.UserName,
                    Email = request.Email,

                };

                var result = await _userManager.CreateAsync(identityUser, request.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    return new AuthModel { Message = errors };

                    //throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));
                }


                var roleResult = await _userManager.AddToRoleAsync(identityUser, "User");

                if (!roleResult.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in roleResult.Errors)
                        errors += $"{error.Description},";

                    return new AuthModel { Message = errors };

                    // throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                businessUser = new User
                {
                    IdentityUserId = identityUser.Id,
                    Email = identityUser.Email,
                    Name = request.UserName,
                    CreatedAt = DateTime.UtcNow,
                };

                await _user.AddAsync(businessUser);

                await _uow.SaveChangesAsync();


                var claimResult = await _userManager.AddClaimAsync(
                    identityUser,
                    new Claim("business_user_id", businessUser.Id.ToString())
                );

                if (!claimResult.Succeeded)
                {

                    var errors = string.Empty;

                    foreach (var error in claimResult.Errors)
                        errors += $"{error.Description},";

                    return new AuthModel { Message = errors };
                }
                //  var jwtToken = await _tokenService.GenerateJwtAsync(identityUser);

                //////////////////////////////////////////////////
                var jwtToken = await _tokenService.GenerateJwtAsync(identityUser);

                var refreshToken = GenerateRefreshToken();

                identityUser.RefreshTokens.Add(refreshToken);

                await _userManager.UpdateAsync(identityUser);



                //////////////////////////////////////////////////
                return new AuthModel
                {
                    Email = identityUser.Email,
                    ExpiresOn = jwtToken.ValidTo,//
                    IsAuthenticated = true,
                    Roles = new List<string> { "User" },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),//
                    RefreshToken = refreshToken.Token,//
                    RefreshTokenExpiration = refreshToken.ExpiresOn,//
                    UserName = identityUser.UserName
                };
            }
            catch
            {

                if (businessUser != null)
                {
                    _user.Delete(businessUser);
                    await _uow.SaveChangesAsync();
                }

                // 2. Delete identity user if created
                if (identityUser != null)
                {
                    await _userManager.DeleteAsync(identityUser);
                }
                throw;
            }

        }

        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var authModel = new AuthModel();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                authModel.Message = "Invalid token";
                return authModel;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                authModel.Message = "Inactive token";
                return authModel;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await _tokenService.GenerateJwtAsync(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authModel;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }

        
        public async Task<bool> RevokeTokenAsync(int userId)
        {
            var user= await _user.FindOneAsync(u=>u.Id==userId);

            if (user == null)
                return false;

            var useridentity = await _userManager.FindByIdAsync(user.IdentityUserId);//.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.IsActive==true));

            if (useridentity == null)
                return false;

            var refreshTokens=useridentity.RefreshTokens.Where(r=>r.IsActive).ToList();

            if (!refreshTokens.Any())
                return false;

            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.RevokedOn = DateTime.UtcNow;
            }

            //var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            //if (!refreshToken.IsActive)
            //    return false;

            //refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(useridentity);

            return true;
        }

        public async Task<responseforgetpass> ForgotPassword(RequestForgotPasswordDto req)
        {
            
            var user =await _userManager.FindByEmailAsync(req.Email);

            if(user == null)
            {
                return new responseforgetpass {errorMessage="invalid Payload" };
            }

            var token =await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(token))
            {
                return new responseforgetpass { errorMessage = "someThing wrong" };
            }

          //   var callbackUrl = $"http://localhost:1000/restpass?code={token}&email={user.Email}";
 

            return new responseforgetpass { email = req.Email,token = token};

        }

        public async Task<string> ResetPassword(ResetPasswordReqDto req)
        {

            var user = await _userManager.FindByEmailAsync(req.Email);

            if (user == null)
            {
                return   "invalid Payload" ;
            }

           var result=await _userManager.ResetPasswordAsync(user,req.Token,req.Password);

      

            if (result.Succeeded)
                return "password restet is successful";

            return "some thing went wrong";

        }
    }
}
