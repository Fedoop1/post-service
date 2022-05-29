using PostService.Common.Types;

namespace PostService.Identity.Messages.Commands;

public record SignIn(string UserName, string Password) : ICommand;
