namespace PostService.Common.Jwt.Types;

public class InvalidAccessTokenException : Exception
{
    public InvalidAccessTokenException(string message) : base(message)
    {
    }
}
