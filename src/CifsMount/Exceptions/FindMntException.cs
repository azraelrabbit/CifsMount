namespace CifsMount.Exceptions;

/// <summary>
/// Failed to check if directory is already mounted via findmnt command
/// </summary>
public class FindMntException : Exception
{
    internal FindMntException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}