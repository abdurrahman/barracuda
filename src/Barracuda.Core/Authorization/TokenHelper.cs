using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Barracuda.Core.Configurations;
using Barracuda.Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Barracuda.Core.Authorization
{
    public class TokenHelper<TUser> : ITokenHelper<TUser>
        where TUser : IdentityUser
    {
        private readonly JwtConfig _jwtConfig;

        public TokenHelper(IOptions<JwtConfig> options)
        {
            _jwtConfig = options.Value;
        }

        public TokenResponseModel GenerateToken(TUser user, IList<string> roles = null)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            return GenerateToken(user, claims, roles);
        }

        public TokenResponseModel GenerateToken(TUser user, IEnumerable<Claim> claims, IList<string> roles = null)
        {
            return GenerateToken(GetTokenClaims(user).Union(claims), roles);
        }

        public TokenResponseModel GenerateToken(IEnumerable<Claim> claims, IList<string> roles = null)
        {
            if (roles != null)
            {
                claims = claims.Union(GetRoleClaims(roles));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryInMinutes),
                claims: claims,
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new TokenResponseModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };
        }

        private static IEnumerable<Claim> GetTokenClaims(IdentityUser user)
        {
            return new List<Claim>
            {
                new Claim(AuthorizationConstants.CLAIM_USERNAME, user.UserName),
                new Claim(AuthorizationConstants.CLAIM_EMAIL, user.Email),
                new Claim(AuthorizationConstants.CLAIM_USERID, user.Id),
                new Claim(AuthorizationConstants.CLAIM_NAME, user.UserName)
            };
        }

        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = System.Text.Encoding.UTF8.GetBytes(_jwtConfig.SecureKey);
            return new SymmetricSecurityKey(symmetricKey);
        }

        private static IEnumerable<Claim> GetRoleClaims(IList<string> roles)
        {
            return roles.Select(c => new Claim(AuthorizationConstants.CLAIM_ROLES, c));
        }
    }
}