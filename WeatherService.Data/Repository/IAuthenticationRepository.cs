using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WeatherService.Data.Repository
{
    public interface IAuthenticationRepository
    {
        (string, bool) ValidateUser(string userName, string password);

        JwtSecurityToken GetTokenForValidUser(List<Claim> authClaims);
    }
}
