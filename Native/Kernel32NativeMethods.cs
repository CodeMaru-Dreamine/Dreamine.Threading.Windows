using System;
using System.Runtime.InteropServices;

namespace Dreamine.Threading.Windows.Native;

/// <summary>
/// \if KO
/// <para>Windows 스레드 작업을 위한 네이티브 <c>kernel32.dll</c> 메서드를 제공합니다.</para>
/// \endif
/// \if EN
/// <para>Provides native <c>kernel32.dll</c> methods for Windows thread operations.</para>
/// \endif
/// </summary>
internal static class Kernel32NativeMethods
{
    /// <summary>
    /// \if KO
    /// <para>현재 스레드의 의사 핸들을 가져옵니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets a pseudo handle for the current thread.</para>
    /// \endif
    /// </summary>
    /// <returns>
    /// \if KO
    /// <para>현재 스레드를 나타내는 의사 핸들입니다.</para>
    /// \endif
    /// \if EN
    /// <para>A pseudo handle representing the current thread.</para>
    /// \endif
    /// </returns>
    /// <exception cref="DllNotFoundException">
    /// \if KO
    /// <para><c>kernel32.dll</c>을 로드할 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when <c>kernel32.dll</c> cannot be loaded.</para>
    /// \endif
    /// </exception>
    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetCurrentThread();

    /// <summary>
    /// \if KO
    /// <para>지정한 스레드에 프로세서 선호도 마스크를 설정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Sets a processor-affinity mask for the specified thread.</para>
    /// \endif
    /// </summary>
    /// <param name="threadHandle">
    /// \if KO
    /// <para>대상 스레드 핸들입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The target thread handle.</para>
    /// \endif
    /// </param>
    /// <param name="threadAffinityMask">
    /// \if KO
    /// <para>적용할 프로세서 선호도 마스크입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The processor-affinity mask to apply.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>이전 선호도 마스크이며 호출 실패 시 0입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The previous affinity mask, or zero when the call fails.</para>
    /// \endif
    /// </returns>
    /// <exception cref="DllNotFoundException">
    /// \if KO
    /// <para><c>kernel32.dll</c>을 로드할 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when <c>kernel32.dll</c> cannot be loaded.</para>
    /// \endif
    /// </exception>
    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern UIntPtr SetThreadAffinityMask(
        IntPtr threadHandle,
        UIntPtr threadAffinityMask);
}
