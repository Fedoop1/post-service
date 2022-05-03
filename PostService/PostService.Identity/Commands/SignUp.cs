using PostService.Common.Enums;
using PostService.Common.Types;

namespace PostService.Identity.Commands;

public record SignUp(string UserName, string Email, string Password, Role Role = Role.User) : ICommand;
