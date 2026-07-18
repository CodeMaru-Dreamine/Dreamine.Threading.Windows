using System.ComponentModel;
using Dreamine.Threading.Interfaces;
using Dreamine.Threading.Windows.Native;

namespace Dreamine.Threading.Windows.Services;

/// <summary>
/// \if KO
/// <para>현재 Windows 스레드에 CPU 선호도를 적용합니다.</para>
/// \endif
/// \if EN
/// <para>Applies CPU affinity to the current Windows thread.</para>
/// \endif
/// </summary>
public sealed class WindowsThreadAffinityService : IThreadAffinityService
{
    /// <summary>
    /// \if KO
    /// <para>현재 Windows 스레드의 선호도 마스크를 지정한 단일 CPU 코어로 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Sets the current Windows thread's affinity mask to one specified CPU core.</para>
    /// \endif
    /// </summary>
    /// <param name="coreIndex">
    /// \if KO
    /// <para>현재 프로세스 비트 폭 안의 0부터 시작하는 CPU 코어 인덱스입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The zero-based CPU core index within the current process bit width.</para>
    /// \endif
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// \if KO
    /// <para><paramref name="coreIndex"/>가 음수이거나 선호도 마스크 비트 폭을 벗어날 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when <paramref name="coreIndex"/> is negative or exceeds the affinity-mask width.</para>
    /// \endif
    /// </exception>
    /// <exception cref="Win32Exception">
    /// \if KO
    /// <para>Windows가 스레드 선호도 마스크 설정을 거부할 때 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when Windows rejects the thread-affinity mask.</para>
    /// \endif
    /// </exception>
    /// <exception cref="DllNotFoundException">
    /// \if KO
    /// <para>Windows 네이티브 라이브러리를 로드할 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when the Windows native library cannot be loaded.</para>
    /// \endif
    /// </exception>
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
    /// \if KO
    /// <para>현재 스레드의 CPU 선호도를 해제합니다. 현재 구현은 이전 마스크를 보존하지 않아 아무 작업도 하지 않습니다.</para>
    /// \endif
    /// \if EN
    /// <para>Clears CPU affinity from the current thread. The current implementation is a no-op because it does not retain the previous mask.</para>
    /// \endif
    /// </summary>
    /// <remarks>
    /// \if KO
    /// <para>실제 복원에는 <see cref="ApplyToCurrentThread"/> 호출 전에 이전 마스크를 스레드별로 보존하는 계약 확장이 필요합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Actual restoration requires a contract extension that preserves the previous mask per thread before <see cref="ApplyToCurrentThread"/>.</para>
    /// \endif
    /// </remarks>
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
