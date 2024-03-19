using System.Text;
using CifsMount.Abstractions;
using CifsMount.Exceptions;
using CifsMount.Models;

namespace CifsMount;

/// <summary>
/// Mounted directory information
/// </summary>
public class CifsMountDirectory : ICifsMountDirectory
{
    /// <summary>
    /// Origin shared directory
    /// </summary>
    public string ShareDirectory { get; }
    /// <summary>
    /// Local mounted directory
    /// </summary>
    public DirectoryInfo Directory { get; }

    /// <summary>
    /// Local mounted directory path
    /// </summary>
    private readonly string _targetDirectory;
    /// <summary>
    /// Mount options
    /// </summary>
    private readonly CifsMountOptions _options;
    /// <summary>
    /// Current user information
    /// </summary>
    private readonly CurrentUserInfo _currentUserInfo;
    /// <summary>
    /// Commands executor
    /// </summary>
    private readonly ICifsMountExecutor _cifsMountExecutor;
    /// <summary>
    /// Mount client
    /// </summary>
    private readonly CifsMountClient _cifsMountClient;
    private readonly bool _useSudo;

    /// <summary>
    /// Create mounted directory
    /// </summary>
    /// <param name="shareDirectory">Origin shared directory</param>
    /// <param name="targetDirectory">Local mounted directory path</param>
    /// <param name="useSudo"></param>
    /// <param name="options">Mount options</param>
    /// <param name="currentUserInfo">Current user information</param>
    /// <param name="cifsMountExecutor">Commands executor</param>
    /// <param name="cifsMountClient">Mount client</param>
    internal CifsMountDirectory(
        string shareDirectory,
        string targetDirectory,
        bool useSudo,
        CifsMountOptions options,
        CurrentUserInfo currentUserInfo,
        ICifsMountExecutor cifsMountExecutor,
        CifsMountClient cifsMountClient)
    {
        ShareDirectory = shareDirectory;
        Directory = new(targetDirectory);
        _targetDirectory = targetDirectory;
        _options = options;
        _currentUserInfo = currentUserInfo;
        _cifsMountExecutor = cifsMountExecutor;
        _cifsMountClient = cifsMountClient;
        _useSudo = useSudo;
    }

    /// <summary>
    /// Perform mount directory
    /// </summary>
    /// <exception cref="UnableCreateMountDirectoryException">Failed to create mount directory</exception>
    /// <exception cref="UnableMountException">Unable mount directory</exception>
    internal void Mount()
    {
        if (Directory.Exists && IsAlreadyMounted())
            return;

        if (!Directory.Exists)
            CreateLocalDirectory();

        var mountArguments = BuildMountArguments();
        var commandArguments = $"-t cifs -o {mountArguments} \"{ShareDirectory}\" \"{_targetDirectory}\"";

        var mountCmdResult = _cifsMountExecutor.RunCommand("mount", commandArguments, _useSudo);
        if (!mountCmdResult.IsSuccessful)
            throw new UnableMountException($"Unable mount directory: {mountCmdResult.Message}");
    }

    /// <summary>
    /// Check if mounted directory already mounted
    /// </summary>
    /// <returns>If already mounted</returns>
    /// <exception cref="FindMntException">Unable check if directory mounted</exception>
    internal bool IsAlreadyMounted()
    {
        var findMntCmdResult = _cifsMountExecutor.RunCommand("findmnt", _targetDirectory);
        if (findMntCmdResult.IsSuccessful)
            return !string.IsNullOrEmpty(findMntCmdResult.Message);
        else
            throw new FindMntException($"Bad response from findmnt command: {findMntCmdResult.Message}");
    }

    /// <summary>
    /// Create local directory for mount
    /// </summary>
    /// <exception cref="UnableCreateMountDirectoryException">Unable mount local directory</exception>
    private void CreateLocalDirectory()
    {
        try
        {
            Directory.Create();
        }
        catch (Exception ex)
        {
            throw new UnableCreateMountDirectoryException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Build arguments for mount command
    /// </summary>
    /// <returns>Arguments</returns>
    private string BuildMountArguments()
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(_options.Credentials))
            sb.Append($"credentials={_options.Credentials},");
        else
        {
            if (!string.IsNullOrEmpty(_options.Username))
                sb.Append($"username={_options.Username},");
            if (!string.IsNullOrEmpty(_options.Password))
                sb.Append($"password={_options.Password},");
            if (!string.IsNullOrEmpty(_options.Domain))
                sb.Append($"domain={_options.Domain},");
        }

        sb.Append($"uid={_currentUserInfo.Uid},");
        sb.Append($"gid={_currentUserInfo.Gid},");

        if (_options.IsReadwrite)
            sb.Append("rw,");

        var validArguments = _options.GetValidArguments();
        sb.Append(string.Join(",", validArguments));

        if (sb.Length > 0)
            sb.Length--;

        return sb.ToString();
    }

    /// <summary>
    /// Perform unmount directory
    /// </summary>
    /// <exception cref="UnableUnmountException">Unable unmount directory</exception>
    private void Unmount()
    {
        var umountCmdResult = _cifsMountExecutor.RunCommand("umount", _targetDirectory, _useSudo);
        if (!umountCmdResult.IsSuccessful)
            throw new UnableUnmountException($"Unable unmount directory: {umountCmdResult.Message}");

        _cifsMountClient.DeleteUnmountedDirectory(_targetDirectory);
    }

    /// <summary>
    /// Unmount directory if not persistence
    /// </summary>
    public void Dispose()
    {
        if (!_options.IsPersistence)
            Unmount();
    }
}