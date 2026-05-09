using System.ComponentModel;
using Dreamine.Threading.Interfaces;
using Dreamine.Threading.Windows.Native;

namespace Dreamine.Threading.Windows.Services;

/// <summary>
/// Applies CPU affinity to the current Windows thread.
/// </summary>
public sealed class WindowsThreadAffinityService : IThreadAffinityService
{
    /// <summary>
    /// Applies CPU affinity to the current thread.
    /// </summary>
    /// <param name="coreIndex">The CPU core index.</param>
    public void ApplyToCurrentThread(int coreIndex)
    {
        if (coreIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coreIndex), "CPU core index cannot be negative.");
        }

        if (coreIndex >= IntPtr.Size * 8)
        {
            throw new ArgumentOutOfRangeException(
                nameof(coreIndex),
                $"CPU core index {coreIndex} exceeds the affinity mask width.");
        }

        var mask = new UIntPtr(1UL << coreIndex);
        var currentThread = Kernel32NativeMethods.GetCurrentThread();

        var previousMask = Kernel32NativeMethods.SetThreadAffinityMask(currentThread, mask);

        if (previousMask == UIntPtr.Zero)
        {
            throw new Win32Exception(
                System.Runtime.InteropServices.Marshal.GetLastWin32Error(),
                $"Failed to set thread affinity. CoreIndex={coreIndex}");
        }
    }

    /// <summary>
    /// Clears CPU affinity from the current thread when supported.
    /// </summary>
    public void ClearCurrentThreadAffinity()
    {
        /*
         * Windows SetThreadAffinityMask requires a valid mask.
         * Restoring the original process-wide scheduling behavior requires the previous
         * affinity mask to be stored per thread.
         *
         * The current Core interface does not expose a restore token, so this method is
         * intentionally a no-op for now.
         *
         * Future improvement:
         * - Change IThreadAffinityService.ApplyToCurrentThread to return a restore handle.
         * - Restore the previous affinity mask in Clear/Dispose.
         */
    }
}