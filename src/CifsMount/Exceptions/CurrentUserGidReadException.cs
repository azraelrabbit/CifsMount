namespace CifsMount.Exceptions;

/// <summary>
/// Failed to parse gid field from id command
/// </summary>
public class CurrentUserGidReadException : CurrentUserReadException
{
    internal CurrentUserGidReadException(Exception? innerException = null)
        : base("Failed to parse gid field from id command", innerException)
    {
    }
}