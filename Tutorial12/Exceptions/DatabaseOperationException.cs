namespace Tutorial12.Exceptions;

public class DatabaseOperationException : Exception
{
    public DatabaseOperationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}