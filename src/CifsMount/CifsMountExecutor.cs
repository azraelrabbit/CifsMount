using System.Diagnostics;
using CifsMount.Abstractions;
using CifsMount.Models;

namespace CifsMount;

/// <summary>
/// Commands executor
/// </summary>
internal class CifsMountExecutor : ICifsMountExecutor
{
    /// <summary>
    /// Max time for wait executing process
    /// </summary>
    private static readonly int WaitExitProcessMsec = 10_000;

    /// <summary>
    /// Execute any local command by system process
    /// </summary>
    /// <param name="command">Command name</param>
    /// <param name="args">Command arguments</param>
    /// <param name="isSudo">Run command as sudo user</param>
    /// <returns>Execute results</returns>
    RunCommandResult ICifsMountExecutor.RunCommand(string command, string? args, bool isSudo)
    {

        var processStartInfo = BuildProcessStartInfo(command, args, isSudo);

#if DEBUG
        Console.WriteLine(processStartInfo.FileName+" | "+processStartInfo.Arguments);
#endif
        var process = StartProcess(processStartInfo);

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit(WaitExitProcessMsec);

        if (string.IsNullOrEmpty(error))
            return new (true, output);
        else
            return new (false, error);
    }

    /// <summary>
    /// Build system process settings
    /// </summary>
    /// <param name="command">Command name</param>
    /// <param name="args">Command arguments</param>
    /// <param name="isSudo">Run command as sudo user</param>
    /// <returns>Process settings</returns>
    private ProcessStartInfo BuildProcessStartInfo(string command, string? args, bool isSudo)
    {
        return new ProcessStartInfo
        {
            FileName = isSudo ? "sudo" : command,
            Arguments = isSudo ? $"{command} {args}" : args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
    }

    /// <summary>
    /// Start system process
    /// </summary>
    /// <param name="processStartInfo">Process settings</param>
    /// <returns>Started process</returns>
    private Process StartProcess(ProcessStartInfo processStartInfo)
    {
        var process = new Process
        {
            StartInfo = processStartInfo
        };

        process.Start();

        return process;
    }
}