using PostService.Common.Types;

namespace PostService.Operations.Models.Domain;

public class Operation : IIdentifiable
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public OperationState State { get; set; }
    public string Reason { get; set; }
}
