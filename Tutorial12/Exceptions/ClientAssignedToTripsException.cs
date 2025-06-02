namespace Tutorial12.Exceptions;

public class ClientAssignedToTripsException : Exception
{
    public ClientAssignedToTripsException(string? message) : base(message)
    {
    }
}