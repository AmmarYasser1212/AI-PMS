using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PMS.Application.Options;
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
    public class TokenServices : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtOptions _jwt;
        
     //   private readonly RefreshTokenOptions _rToken;

        public TokenServices(
            UserManager<AppUser> userManager,
            IOptions<JwtOptions> jwtOptions
            /*IOptions<RefreshTokenOptions> rOptions*/)
        {
            _userManager = userManager;
            _jwt = jwtOptions.Value;
           // _rToken = rOptions.Value;
        }

        public async Task<SecurityToken> GenerateJwtAsync(AppUser user)
        {
            var userClaims = await _userManager
          .GetClaimsAsync(user);

            var userRoles = await _userManager
                .GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName!));
            userClaims.Add(new Claim(ClaimTypes.Email, user.Email!));

            // prepare signingCredentials
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwt.SigningKey)
                ), SecurityAlgorithms.HmacSha256
            );

            // tokenDescriptor Contains some information which used to create a security token.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwt.Issuer,
                Audience = _jwt.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwt.Lifetime),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(userClaims)
            };

            // create token
            // A SecurityTokenHandler designed for creating and validating Json Web Tokens.
            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor); // create token with info in tokenDescriptor
            return securityToken;
        }

        public void RevokeAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        //private static string GenerateRefreshToken()
        //{
        //    var randomBytes = RandomNumberGenerator.GetBytes(64);
        //    return Convert.ToBase64String(randomBytes);
        //}

        //private RefreshToken GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[32];

        //    using var generator = new RNGCryptoServiceProvider();

        //    generator.GetBytes(randomNumber);

        //    return new RefreshToken
        //    {
        //        Token = Convert.ToBase64String(randomNumber),
        //        ExpiresOn = DateTime.UtcNow.AddMinutes(_jwt.Lifetime),
        //        CreatedOn = DateTime.UtcNow
        //    };
        //}
    }
}
