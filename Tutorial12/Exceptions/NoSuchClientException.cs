namespace Tutorial12.Exceptions;

public class NoSuchClientException : Exception
{
    public NoSuchClientException(string? message) : base(message)
    {
    }
}