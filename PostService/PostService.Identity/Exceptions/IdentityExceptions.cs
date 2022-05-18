namespace PostService.Identity.Exceptions;

public class InvalidUserException : Exception
{
    public InvalidUserException(string message) : base(message)
    {
    }
}

public class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException(string message) : base(message)
    {
    }
}



public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) : base(message)
    {
    }
}
