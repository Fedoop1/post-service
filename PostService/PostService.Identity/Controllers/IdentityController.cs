using Microsoft.AspNetCore.Mvc;
using PostService.Identity.Services;
using PostService.Common.Jwt.Extensions;
using PostService.Identity.Messages.Commands;

namespace PostService.Identity.Controllers
{
    [Route("")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUp command)
        {
            await this.identityService.SignUpAsync(command.UserName, command.Email, command.Password, command.Role);

            return NoContent();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignIn command) =>
            Ok(await this.identityService.SignInAsync(command.UserName, command.Password));

        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOut(SignOut command)
        {
            await this.identityService.SignOutAsync(command.UserId,
                JwtExtensions.GetBearerToken(this.Request.Headers.Authorization));

            return NoContent();
        }

        
    }
}
