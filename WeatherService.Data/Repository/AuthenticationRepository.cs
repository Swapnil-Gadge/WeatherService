using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherService.Data.Validation;

namespace WeatherService.Data.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ParentUserValidation userValidator;
        private readonly IConfiguration configuration;

        public AuthenticationRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.userValidator = new NormalUserValidation(this.configuration["USER:Password"]);
            ParentUserValidation adminValidator = new AdminUserValidation(this.configuration["ADMIN:Password"]);
            ParentUserValidation superAdminValidator = new SuperAdminValidation(this.configuration["SUPERADMIN:Password"]);
            this.userValidator.Parent = adminValidator;
            adminValidator.Parent = superAdminValidator;
        }

        public JwtSecurityToken GetTokenForValidUser(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public (string, bool) ValidateUser(string userName, string password)
        {
            return this.userValidator.ValidateUser(userName, password);
        }
    }
}
