namespace PostService.Identity.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException(string message) : base(message)
    {
    }
}

public class InvalidEmailException : Exception
{
    public InvalidEmailException(string message) : base(message)
    {
    }
}

public class InvalidNameException : Exception
{
    public InvalidNameException(string message) : base(message)
    {
    }
}

public class InvalidPasswordHasherException : Exception
{
    public InvalidPasswordHasherException(string message) : base(message)
    {
    }
}

public class UserAlreadyInUseException : Exception
{
    public UserAlreadyInUseException(string message) : base(message)
    {
    }
}

public class UserNotExistsException : Exception
{
    public UserNotExistsException(string message) : base(message)
    {
    }
}

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) : base(message)
    {
    }
}

public class InvalidUserException : Exception
{
    public InvalidUserException(string message) : base(message)
    {
    }
}

public class TokenAlreadyRevokedException : Exception
{
    public TokenAlreadyRevokedException(string message) : base(message)
    {
    }
}
