using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WeatherService.Domain.Service
{
    public interface IAuthentiationService
    {
        (string, bool) ValidateUser(string userName, string password);
        JwtSecurityToken GetTokenForValidUser(List<Claim> authClaims);
    }
}
