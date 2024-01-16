using Demo.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Demo.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Authenticate(string user, string pwd)
        {
            if (!(user == "admin" && pwd == "password"))
            {
                ModelState.AddModelError("Unauthorized", "You are unthorized");
                return Unauthorized(ModelState);
            }

            var claims = new[]
              {
                    new Claim(ClaimTypes.Name, user),
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["Jwt:ExpiryInDays"]));

            var token = new JwtSecurityToken(
                Convert.ToString(_configuration["Jwt:Issuer"]),
                Convert.ToString(_configuration["Jwt:Audience"]),
                claims,
                expires: expiry,
                signingCredentials: creds  
                );

            return Ok(new LoginResponse()
            {
                Successful = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
