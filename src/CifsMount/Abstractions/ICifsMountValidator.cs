namespace CifsMount.Abstractions;

/// <summary>
/// Interface for validate execute context
/// </summary>
internal interface ICifsMountValidator
{
    /// <summary>
    /// Check current system environment and throw if is not compatible
    /// </summary>
    internal void ThrowIfNotPossibleExecuteInCurrentSystemEnvironment();
}