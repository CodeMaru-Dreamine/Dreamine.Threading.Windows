using System.Runtime.InteropServices;

namespace Dreamine.Threading.Windows.Native;

/// <summary>
/// \if KO
/// <para>타이머 해상도 제어를 위한 네이티브 <c>winmm.dll</c> 메서드를 제공합니다.</para>
/// \endif
/// \if EN
/// <para>Provides native <c>winmm.dll</c> methods for timer-resolution control.</para>
/// \endif
/// </summary>
internal static class WinMmNativeMethods
{
    /// <summary>
    /// \if KO
    /// <para>최소 타이머 해상도를 요청합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Requests a minimum timer resolution.</para>
    /// \endif
    /// </summary>
    /// <param name="period">
    /// \if KO
    /// <para>밀리초 단위 요청 해상도입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The requested resolution in milliseconds.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>성공 시 0, 실패 시 오류 코드입니다.</para>
    /// \endif
    /// \if EN
    /// <para>Zero on success; otherwise, an error code.</para>
    /// \endif
    /// </returns>
    /// <exception cref="DllNotFoundException">
    /// \if KO
    /// <para><c>winmm.dll</c>을 로드할 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when <c>winmm.dll</c> cannot be loaded.</para>
    /// \endif
    /// </exception>
    [DllImport("winmm.dll")]
    internal static extern uint timeBeginPeriod(uint period);

    /// <summary>
    /// \if KO
    /// <para>이전에 요청한 타이머 해상도를 해제합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Clears a previously requested timer resolution.</para>
    /// \endif
    /// </summary>
    /// <param name="period">
    /// \if KO
    /// <para>밀리초 단위 해제할 타이머 해상도입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The timer resolution in milliseconds to clear.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>성공 시 0, 실패 시 오류 코드입니다.</para>
    /// \endif
    /// \if EN
    /// <para>Zero on success; otherwise, an error code.</para>
    /// \endif
    /// </returns>
    /// <exception cref="DllNotFoundException">
    /// \if KO
    /// <para><c>winmm.dll</c>을 로드할 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when <c>winmm.dll</c> cannot be loaded.</para>
    /// \endif
    /// </exception>
    [DllImport("winmm.dll")]
    internal static extern uint timeEndPeriod(uint period);
}
