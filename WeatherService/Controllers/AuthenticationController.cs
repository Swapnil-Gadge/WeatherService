using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherService.Data.Model;
using WeatherService.Domain.Service;

namespace WeatherService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentiationService authentiationService;

        public AuthenticationController(IAuthentiationService authentiationService)
        {
            this.authentiationService = authentiationService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {   
            string userName; 
            bool validUser;

            (userName, validUser) = this.authentiationService.ValidateUser(model.UserName, model.Password);

            if (!validUser)
                return this.Unauthorized();

            var authClaims = new List<Claim>();
            authClaims.Add(new Claim(userName, userName));

            var token = this.authentiationService.GetTokenForValidUser(authClaims);
            
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}