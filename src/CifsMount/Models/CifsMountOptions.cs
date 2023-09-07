namespace CifsMount.Models;

/// <summary>
/// Mount directory options
/// </summary>
public class CifsMountOptions
{
    /// <summary>
    /// Specifies a file that contains a username and/or password
    /// </summary>
    internal readonly string? Credentials;
    /// <summary>
    /// Specifies the username to connect as
    /// </summary>
    internal readonly string? Username;
    /// <summary>
    /// Specifies the CIFS password
    /// </summary>
    internal readonly string? Password;
    /// <summary>
    /// Sets the domain (workgroup) of the user
    /// </summary>
    internal string? Domain { get; set; }
    /// <summary>
    /// Mount read-write
    /// </summary>
    public bool IsReadwrite { get; set; } = true;
    /// <summary>
    /// Whether to disable a directory automatically
    /// </summary>
    public bool IsPersistence { get; set; } = false;
    /// <summary>
    /// Extra arguments for command mount.cifs
    /// All options see in: https://www.samba.org/~ab/output/htmldocs/manpages-3/mount.cifs.8.html
    /// </summary>
    public string[] Arguments { get; set; } = Array.Empty<string>();

    /// <summary>
    /// List of system argument names
    /// (specified by public properties)
    /// </summary>
    private static readonly string[] InternalArgumentKeys = { "credentials", "username", "domain", "password", "uid", "gid", "rw" };

    /// <summary>
    /// Create mount options
    /// </summary>
    public CifsMountOptions() { }

    /// <summary>
    /// Create mount options
    /// </summary>
    /// <param name="credentials">Specifies a file that contains a username and/or password</param>
    public CifsMountOptions(string credentials)
    {
        Credentials = credentials;
    }

    /// <summary>
    /// Create mount options
    /// </summary>
    /// <param name="username">Specifies the username to connect as</param>
    /// <param name="password">Specifies the CIFS password</param>
    /// <param name="domain">Sets the domain (workgroup) of the user</param>
    public CifsMountOptions(string username, string password, string? domain = null)
    {
        Username = username;
        Password = password;
        Domain = domain;
    }

    /// <summary>
    /// Gets properties that are not set system-wide
    /// </summary>
    /// <returns>List of extra arguments</returns>
    internal IEnumerable<string> GetValidArguments()
    {
        foreach (var argument in Arguments)
        {
            if (argument.Contains('='))
            {
                var argumentParts = argument.Split('=');
                var argumentKeyLower = argumentParts[0].ToLower();
                if (!InternalArgumentKeys.Contains(argumentKeyLower))
                    yield return $"{argumentKeyLower}={argumentParts[1]}";
            }
            else
            {
                if (!InternalArgumentKeys.Contains(argument.ToLower()))
                    yield return argument;
            }
        }
    }
}