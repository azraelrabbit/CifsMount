using System.Runtime.InteropServices;
using CifsMount.Abstractions;

namespace CifsMount;

/// <summary>
/// Execute context validator
/// </summary>
internal class CifsMountValidator : ICifsMountValidator
{
    /// <summary>
    /// Check current system environment and throw if is not compatible
    /// </summary>
    void ICifsMountValidator.ThrowIfNotPossibleExecuteInCurrentSystemEnvironment()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            throw new PlatformNotSupportedException("Mounting directories using the library is only possible on Linux");
    }
}