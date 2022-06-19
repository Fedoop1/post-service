using Microsoft.AspNetCore.Mvc;
using PostService.Operations.Services;

namespace PostService.Operations.Controllers;

[Route("[controller]")]
[ApiController]
public class OperationsController : ControllerBase
{
    private readonly IOperationStorage operationStorage;

    public OperationsController(IOperationStorage operationStorage)
    {
        this.operationStorage = operationStorage;
    }

    [HttpGet("{operationId:guid}")]
    public async Task<IActionResult> Get(Guid operationId)
    {
        var result = await this.operationStorage.GetAsync(operationId);
        return result is not null ? Ok(result) : NotFound();
    }
}
