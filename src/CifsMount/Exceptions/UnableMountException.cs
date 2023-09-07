namespace CifsMount.Exceptions;

/// <summary>
/// Failed to mount directory
/// </summary>
public class UnableMountException : Exception
{
    internal UnableMountException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}