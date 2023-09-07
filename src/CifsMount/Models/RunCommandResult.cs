namespace CifsMount.Models;

/// <summary>
/// Result of executing command in Linux terminal
/// </summary>
internal class RunCommandResult
{
    internal readonly bool IsSuccessful;
    internal readonly string Message;

    /// <summary>
    /// Create information about executed command
    /// </summary>
    /// <param name="isSuccessful">Successful command execution</param>
    /// <param name="message">Output message or error</param>
    internal RunCommandResult(bool isSuccessful, string message)
    {
        IsSuccessful = isSuccessful;
        Message = message;
    }
}