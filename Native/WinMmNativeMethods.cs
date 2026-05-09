using System.Runtime.InteropServices;

namespace Dreamine.Threading.Windows.Native;

/// <summary>
/// Provides native winmm.dll methods for timer resolution control.
/// </summary>
internal static class WinMmNativeMethods
{
    /// <summary>
    /// Requests a minimum timer resolution.
    /// </summary>
    /// <param name="period">The requested timer resolution in milliseconds.</param>
    /// <returns>Zero if successful; otherwise an error code.</returns>
    [DllImport("winmm.dll")]
    internal static extern uint timeBeginPeriod(uint period);

    /// <summary>
    /// Clears a previously requested timer resolution.
    /// </summary>
    /// <param name="period">The timer resolution in milliseconds.</param>
    /// <returns>Zero if successful; otherwise an error code.</returns>
    [DllImport("winmm.dll")]
    internal static extern uint timeEndPeriod(uint period);
}