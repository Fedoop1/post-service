using Microsoft.AspNetCore.Mvc;
using PostService.API.Models.Operations;
using PostService.API.Services;

namespace PostService.API.Controllers;

[Route("[controller]")]
[ApiController]
public class OperationsController : ControllerBase
{
    private readonly IOperationsService operationsService;

    public OperationsController(IOperationsService operationsService)
    {
        this.operationsService = operationsService;
    }

    [HttpGet("{id:guid}")]
    public async Task<Operation> GetAsync(Guid id) => await this.operationsService.GetAsync(id);
}
