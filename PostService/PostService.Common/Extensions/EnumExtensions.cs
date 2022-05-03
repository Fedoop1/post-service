using PostService.Common.Enums;
using PostService.Common.Exceptions;

namespace PostService.Common.Extensions;

public static class EnumExtensions
{
    public static string ConvertToString(this Role role) => role switch
    {
        Role.User => "User",
        Role.Admin => "Admin",
        _ => throw new EnumTransformationException($"There isn't role with value {role}"),
    };
}


