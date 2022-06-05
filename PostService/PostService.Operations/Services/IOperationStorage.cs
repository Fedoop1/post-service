﻿using PostService.Operations.Models.Domain;

namespace PostService.Operations.Services;

public interface IOperationStorage
{
    public Task SetAsync(Operation operation);
    public Task<Operation> GetAsync(Guid id);
}
