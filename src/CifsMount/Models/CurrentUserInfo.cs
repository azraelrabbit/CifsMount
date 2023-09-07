namespace CifsMount.Models;

/// <summary>
/// Information about the current user under which the process is running
/// </summary>
internal class CurrentUserInfo
{
    /// <summary>
    /// Sets the uid that will own all files on the mounted filesystem
    /// </summary>
    internal readonly int Uid;
    /// <summary>
    /// Sets the gid that will own all files on the mounted filesystem
    /// </summary>
    internal readonly int Gid;

    /// <summary>
    /// Create user information
    /// </summary>
    /// <param name="uid">Sets the uid that will own all files on the mounted filesystem</param>
    /// <param name="gid">Sets the gid that will own all files on the mounted filesystem</param>
    internal CurrentUserInfo(int uid, int gid)
    {
        Uid = uid;
        Gid = gid;
    }
}