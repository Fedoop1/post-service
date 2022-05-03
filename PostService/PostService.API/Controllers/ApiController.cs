using Microsoft.AspNetCore.Mvc;
using PostService.API.Infrastructure.Filters;
using PostService.API.Infrastructure.Options;

namespace PostService.API.Controllers
{
    [JwtAuth]
    [Route("")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly AppOptions appOptions;

        public ApiController(AppOptions appOptions) => this.appOptions = appOptions;

        [HttpGet]
        public IActionResult Get() => Ok(appOptions.Name);
    }
}
