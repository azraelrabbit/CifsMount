using CifsMount.Models;

namespace CifsMount.Abstractions;

/// <summary>
/// Interface for command executor
/// </summary>
internal interface ICifsMountExecutor
{
    /// <summary>
    /// Execute any local command by system process
    /// </summary>
    /// <param name="command">Command name</param>
    /// <param name="args">Command arguments</param>
    /// <param name="isSudo">Run command as sudo user</param>
    /// <returns>Execute results</returns>
    internal RunCommandResult RunCommand(string command, string? args = null, bool isSudo = false);
}