namespace CifsMount.Abstractions;

/// <summary>
/// Interface for the mount client
/// </summary>
public interface ICifsMountClient
{
    /// <summary>
    /// Mount shared directory to local
    /// </summary>
    /// <param name="shareDirectory">Shared directory in format: //server/RootFolder/SubFolder</param>
    /// <param name="localDirectory">Local directory where the shared directory will be mounted in dormet: /mnt/test</param>
 
    /// <returns>Mount client</returns>
    ICifsMountDirectory Mount(string shareDirectory, string localDirectory);
}