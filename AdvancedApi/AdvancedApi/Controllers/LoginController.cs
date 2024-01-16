using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdvancedApi.Controllers
{
    //[Authorize]
    //because using both identity and JWT provider to authenticate, tell server which one to use
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userInManager;

        public LoginController(IConfiguration config, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _config = config;
            _signInManager = signInManager;
            _userInManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var user = Authenticate(login.UserName, login.Password);
            //if(login.UserName == "admin" && login.Password == "password")
            if(user != null)
            {
                var token = GenerateToken(login.UserName);
                return Ok(token);
            }
            return BadRequest("Login fail");
        }

        private async Task<IdentityUser> Authenticate(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
            if (result.Succeeded)
            {
                var currentUser = await _userInManager.FindByNameAsync(userName);
                if(currentUser != null)
                {
                    return currentUser;
                }
            }
            return null;
        }

        private string GenerateToken(string userName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userName)
            };

            var token = new JwtSecurityToken(
                _config.GetSection("Jwt:Issuer").Value,
                _config.GetSection("Jwt:Audience").Value,
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("Token Successfully Validated");
        }
    }
}
