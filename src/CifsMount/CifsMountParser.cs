using System.Text.RegularExpressions;
using CifsMount.Abstractions;
using CifsMount.Exceptions;
using CifsMount.Models;

namespace CifsMount;

/// <summary>
/// Command result parser
/// </summary>
internal class CifsMountParser : ICifsMountParser
{
    /// <summary>
    /// Parse current user information
    /// </summary>
    /// <param name="idMessage">Result of id command</param>
    /// <returns>Current user information</returns>
    CurrentUserInfo ICifsMountParser.ParseCurrentUserInfo(string idMessage)
    {
        var uid = ParseUid(idMessage);
        var gid = ParseGid(idMessage);

        return new (uid, gid);
    }

    /// <summary>
    /// Parse uid field
    /// </summary>
    /// <param name="idMessage">Result of id command</param>
    /// <returns>Uid of current user</returns>
    /// <exception cref="CurrentUserUidReadException">Unablde parse uid</exception>
    private int ParseUid(string idMessage)
    {
        var uidMatch = new Regex(@"uid=(\d+)").Match(idMessage);
        if (!uidMatch.Success || !int.TryParse(uidMatch.Groups[1].Value, out var uid))
            throw new CurrentUserUidReadException();

        return uid;
    }

    /// <summary>
    /// Parse gid field
    /// </summary>
    /// <param name="idMessage">Result of id command</param>
    /// <returns>Gid of current user</returns>
    /// <exception cref="CurrentUserGidReadException">Unablde parse gid</exception>
    private int ParseGid(string idMessage)
    {
        var gidMatch = new Regex(@"gid=(\d+)").Match(idMessage);
        if (!gidMatch.Success || !int.TryParse(gidMatch.Groups[1].Value, out var gid))
            throw new CurrentUserGidReadException();

        return gid;
    }
}