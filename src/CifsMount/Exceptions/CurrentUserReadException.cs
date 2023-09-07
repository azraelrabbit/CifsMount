namespace CifsMount.Exceptions;

/// <summary>
/// Failed to get current user inforamtion from id command
/// </summary>
public class CurrentUserReadException : Exception
{
    internal CurrentUserReadException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}