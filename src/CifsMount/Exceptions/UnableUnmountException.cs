namespace CifsMount.Exceptions;

/// <summary>
/// Failed to unmount directory
/// </summary>
public class UnableUnmountException : Exception
{
    internal UnableUnmountException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}