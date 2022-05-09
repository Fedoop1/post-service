using Microsoft.AspNetCore.Mvc;
using PostService.Identity.Commands;
using PostService.Identity.Services;

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
    }
}
