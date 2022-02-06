using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WeatherService.Data.Repository;

namespace WeatherService.Domain.Service
{
    public class AuthentiationService : IAuthentiationService
    {
        public readonly IAuthenticationRepository authenticationRepository;

        public AuthentiationService(IAuthenticationRepository authenticationRepository)
        {
            this.authenticationRepository = authenticationRepository;
        }

        public (string, bool) ValidateUser(string userName, string password)
        {
            return this.authenticationRepository.ValidateUser(userName, password);
        }

        public JwtSecurityToken GetTokenForValidUser(List<Claim> authClaims)
        {
            return this.authenticationRepository.GetTokenForValidUser(authClaims);
        }
    }
}
