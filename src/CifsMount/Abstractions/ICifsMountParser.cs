using CifsMount.Models;

namespace CifsMount.Abstractions;

/// <summary>
/// Interface for command result parser
/// </summary>
internal interface ICifsMountParser
{
    /// <summary>
    /// Parse current user information
    /// </summary>
    /// <param name="idMessage">Result of id command</param>
    /// <returns>Current user information</returns>
    internal CurrentUserInfo ParseCurrentUserInfo(string idMessage);
}