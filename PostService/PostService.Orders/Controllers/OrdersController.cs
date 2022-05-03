using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PostService.Orders.Infrastructure.Options;

namespace PostService.Orders.Controllers
{
    [Route("")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppOptions appOptions;

        public OrdersController(AppOptions appOptions)
        {
            this.appOptions = appOptions;
        }

        [HttpGet]
        public IActionResult Get() => Ok(appOptions.Name);
    }
}
