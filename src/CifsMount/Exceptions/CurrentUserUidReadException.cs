namespace CifsMount.Exceptions;

/// <summary>
/// Failed to parse uid field from id command
/// </summary>
public class CurrentUserUidReadException : CurrentUserReadException
{
    internal CurrentUserUidReadException(Exception? innerException = null)
        : base("Failed to parse uid field from id command", innerException)
    {
    }
}