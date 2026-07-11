using Azure.Core;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using PMS.Application.DTO.Auth;
using PMS.Application.DTO.External;
using PMS.Application.Interfaces.Repositories;
using PMS.Application.Interfaces.Services;
using PMS.Domain.Entities;
using PMS.Infrastructre.Data;
using PMS.Infrastructre.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Infrastructre.Services.AuthService
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly Irepsitory<User> _user;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IunitOfWork _uow;
        private readonly ITokenService _tokenService;
        public GoogleAuthService(
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
        public async Task<AuthModel> LoginWithGoogleAsync(string idToken)
        {
            User? businessUser = null;
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            if (payload == null)
                return new AuthModel { IsAuthenticated = false };
            // 2. Find user
            var user = await _userManager.FindByEmailAsync(payload.Email);

            // 3. Create user if not exists
            if (user == null)
            {

                user = new AppUser
                {
                    UserName = payload.Email.Split('@')[0],
                    Email = payload.Email,
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");

                businessUser = new User
                {
                    IdentityUserId = user.Id,
                    Email = user.Email,
                    Name = user.UserName,
                    CreatedAt = DateTime.UtcNow,
                };

                await _user.AddAsync(businessUser);

                await _uow.SaveChangesAsync();


                var claimResult = await _userManager.AddClaimAsync(
                    user,
                    new Claim("business_user_id", businessUser.Id.ToString())
                );

                if (!claimResult.Succeeded)
                {

                    var errors = string.Empty;

                    foreach (var error in claimResult.Errors)
                        errors += $"{error.Description},";

                    return new AuthModel { Message = errors };
                }

            }

            // 4. Generate JWT
            var jwt = await _tokenService.GenerateJwtAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            // 5. Generate Refresh Token
            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new AuthModel
            {
                ExpiresOn = jwt.ValidTo,//
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList()
            };
            
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
    }
}
