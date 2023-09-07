namespace CifsMount.Abstractions;

/// <summary>
/// Interface for the mounted directory
/// </summary>
public interface ICifsMountDirectory : IDisposable
{
    /// <summary>
    /// Origin shared directory
    /// </summary>
    string ShareDirectory { get; }
    /// <summary>
    /// Local mounted directory
    /// </summary>
    public DirectoryInfo Directory { get; }
}