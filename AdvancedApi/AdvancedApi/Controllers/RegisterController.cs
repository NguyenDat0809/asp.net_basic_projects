using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userInManager;
        public RegisterController(UserManager<IdentityUser> userInManager)
        {
            _userInManager = userInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(string userName, string password)
        {
            IdentityUser user = new IdentityUser(userName);
            var result = await _userInManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return Ok("User Register Success");
            }
            return BadRequest("User Register Failed");
        }
    }
}
