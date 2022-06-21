namespace PostService.Common.LoadBalancing.Exceptions;

public class HttpClientException : Exception
{
    public HttpClientException(string message) : base(message)
    {
    }
}

public class ServiceProviderException : Exception
{
    public ServiceProviderException(string message) : base(message)
    {
    }
}
