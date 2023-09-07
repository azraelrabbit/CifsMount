namespace CifsMount.Exceptions;

/// <summary>
/// Failed to create directory for mount
/// </summary>
public class UnableCreateMountDirectoryException : Exception
{
    internal UnableCreateMountDirectoryException(string message, Exception? innerException = null)
        : base($"Unable create mount directory: {message}", innerException)
    {
    }
}