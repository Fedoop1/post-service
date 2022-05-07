using Microsoft.AspNetCore.Mvc;
using PostService.API.Infrastructure.Filters;
using PostService.Identity.Commands;
using PostService.Identity.Infrastructure.Options;
using PostService.Identity.Services.Interfaces;

namespace PostService.Identity.Controllers
{
    [Route("")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly AppOptions appOptions;
        private readonly IIdentityService identityService;

        public IdentityController(AppOptions appOptions, IIdentityService identityService)
        {
            this.appOptions = appOptions;
            this.identityService = identityService;
        }

        [HttpGet("")]
        public IActionResult Get() => Ok(this.appOptions.Name);


        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUp command)
        {
            await this.identityService.SignUpAsync(command.UserName, command.Email, command.Password, command.Role);

            return NoContent();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignIn command) =>
            Ok(await this.identityService.SignInAsync(command.UserName, command.Password));

        [JwtAuth]
        [HttpGet]
        public async Task<IActionResult> GetAccessToken(string refreshToken) => Ok(await this.identityService.GetAccessToken(refreshToken));
    }
}
