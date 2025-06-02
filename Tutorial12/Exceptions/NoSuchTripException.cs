namespace Tutorial12.Exceptions;

public class NoSuchTripException : Exception
{
    public NoSuchTripException(string? message) : base(message)
    {
    }
}