using System.Collections.Concurrent;
using CifsMount.Abstractions;
using CifsMount.Exceptions;
using CifsMount.Models;

namespace CifsMount;

/// <summary>
/// Provides a class for mount directories
/// </summary>
public class CifsMountClient : ICifsMountClient, IDisposable
{
    private readonly bool _useSudo;

    /// <summary>
    /// Mount options
    /// </summary>
    private readonly CifsMountOptions _options;
    /// <summary>
    /// Commands executor
    /// </summary>
    private readonly ICifsMountExecutor _cifsMountExecutor;
    /// <summary>
    /// Command results parser
    /// </summary>
    private readonly ICifsMountParser _cifsMountParser;
    /// <summary>
    /// Validator for execute context
    /// </summary>
    private readonly ICifsMountValidator _cifsMountValidator;
    /// <summary>
    /// List of mounted directories by current client
    /// </summary>
    private readonly ConcurrentDictionary<string, ICifsMountDirectory> _mountedDirectories = new();
    /// <summary>
    /// Information about current user
    /// </summary>
    private CurrentUserInfo? _currentUserInfo;
    /// <summary>
    /// Current user inforamtion inited
    /// </summary>
    private bool _isInited;

    /// <summary>
    /// Create client with options
    /// </summary>
    /// <param name="useSudo">whether you need use sudo</param>
    /// <param name="options">Mount options</param>
    public CifsMountClient(bool useSudo,CifsMountOptions? options = null)
        : this(options ?? new(),
            new CifsMountExecutor(),
            new CifsMountParser(),
            new CifsMountValidator())
    {
        _useSudo = useSudo;
    }

    /// <summary>
    /// Create client with options, executor and parser
    /// (for tests)
    /// </summary>
    /// <param name="options">Mount options</param>
    /// <param name="cifsMountExecutor">Commands executor</param>
    /// <param name="cifsMountParser">Command results parser</param>
    /// <param name="cifsMountValidator">Validator for execute context</param>
    internal CifsMountClient(CifsMountOptions options,
        ICifsMountExecutor cifsMountExecutor,
        ICifsMountParser cifsMountParser,
        ICifsMountValidator cifsMountValidator)
    {
        _options = options;
        _cifsMountExecutor = cifsMountExecutor;
        _cifsMountParser = cifsMountParser;
        _cifsMountValidator = cifsMountValidator;
    }

    /// <summary>
    /// Mount shared directory to local
    /// </summary>
    /// <param name="shareDirectory">Shared directory in format: //server/RootFolder/SubFolder</param>
    /// <param name="localDirectory">Local directory where the shared directory will be mounted</param>
 
    /// <returns>Mount client</returns>
    /// <exception cref="PlatformNotSupportedException">Only running on Linux</exception>
    public ICifsMountDirectory Mount(string shareDirectory, string localDirectory)
    {
        _cifsMountValidator.ThrowIfNotPossibleExecuteInCurrentSystemEnvironment();

        if (!_isInited)
            Init();

        if (_mountedDirectories.TryGetValue(localDirectory, out var alreadyMountedDirecotry) &&
            ((CifsMountDirectory)alreadyMountedDirecotry).IsAlreadyMounted())
            return alreadyMountedDirecotry;

        var mountedDirectory = new CifsMountDirectory(
            shareDirectory,
            localDirectory,
            _useSudo,
            _options,
            _currentUserInfo!,
            _cifsMountExecutor,
            this);
        mountedDirectory.Mount();

        _mountedDirectories[localDirectory] = mountedDirectory;

        return mountedDirectory;
    }

    /// <summary>
    /// Delete unmounted directory from list
    /// </summary>
    /// <param name="unmountedDirectory"></param>
    internal void DeleteUnmountedDirectory(string unmountedDirectory) =>
        _mountedDirectories.TryRemove(unmountedDirectory, out _);

    /// <summary>
    /// Init current user inforamtion
    /// </summary>
    /// <exception cref="CurrentUserReadException">Bad response from id command</exception>
    private void Init()
    {
        var idCmdResult = _cifsMountExecutor.RunCommand("id");
        if (idCmdResult.IsSuccessful && !string.IsNullOrEmpty(idCmdResult.Message))
            _currentUserInfo = _cifsMountParser.ParseCurrentUserInfo(idCmdResult.Message);
        else
            throw new CurrentUserReadException($"Bad response from id command: {idCmdResult.Message}");

        _isInited = true;
    }

    /// <summary>
    /// Unmount directories if not persistence
    /// </summary>
    public void Dispose()
    {
        foreach (var mountedDirectoriy in _mountedDirectories.Values)
            mountedDirectoriy.Dispose();
    }
}