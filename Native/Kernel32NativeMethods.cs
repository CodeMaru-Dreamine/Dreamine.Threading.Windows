using System;
using System.Runtime.InteropServices;

namespace Dreamine.Threading.Windows.Native;

/// <summary>
/// Provides native kernel32.dll methods for Windows thread operations.
/// </summary>
internal static class Kernel32NativeMethods
{
    /// <summary>
    /// Gets a pseudo handle for the current thread.
    /// </summary>
    /// <returns>A pseudo handle for the current thread.</returns>
    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetCurrentThread();

    /// <summary>
    /// Sets a processor affinity mask for the specified thread.
    /// </summary>
    /// <param name="threadHandle">The thread handle.</param>
    /// <param name="threadAffinityMask">The processor affinity mask.</param>
    /// <returns>The previous affinity mask, or zero if the call failed.</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern UIntPtr SetThreadAffinityMask(
        IntPtr threadHandle,
        UIntPtr threadAffinityMask);
}