using PostService.Common.Types;

namespace PostService.Identity.Commands;

public record SignIn(string UserName, string Password) : ICommand;
