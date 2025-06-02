namespace Tutorial12.Exceptions;

public class TripAlreadyHappenedException : Exception
{
    public TripAlreadyHappenedException(string? message) : base(message)
    {
    }
}