using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Barracuda.Core.Authorization
{
    public interface ITokenHelper<TUser>
        where TUser : IdentityUser
    {
        TokenResponseModel GenerateToken(TUser user, IList<string> roles = null);

        TokenResponseModel GenerateToken(TUser user, IEnumerable<Claim> claims, IList<string> roles = null);

        TokenResponseModel GenerateToken(IEnumerable<Claim> claims, IList<string> roles = null);
    }
}