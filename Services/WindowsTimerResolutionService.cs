using Dreamine.Threading.Interfaces;
using Dreamine.Threading.Windows.Native;

namespace Dreamine.Threading.Windows.Services;

/// <summary>
/// \if KO
/// <para>고정밀 스레드 주기를 위해 Windows 타이머 해상도를 참조 카운트 방식으로 제어합니다.</para>
/// \endif
/// \if EN
/// <para>Controls Windows timer resolution with reference counting for high-precision thread cycles.</para>
/// \endif
/// </summary>
public sealed class WindowsTimerResolutionService : ITimerResolutionService, IDisposable
{
    /// <summary>
    /// \if KO
    /// <para>sync Root 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the sync root value.</para>
    /// \endif
    /// </summary>
    private readonly object _syncRoot = new();
    /// <summary>
    /// \if KO
    /// <para>period 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the period value.</para>
    /// \endif
    /// </summary>
    private readonly uint _period;
    /// <summary>
    /// \if KO
    /// <para>reference Count 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the reference count value.</para>
    /// \endif
    /// </summary>
    private int _referenceCount;
    /// <summary>
    /// \if KO
    /// <para>disposed 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the disposed value.</para>
    /// \endif
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// \if KO
    /// <para>기본 1밀리초 주기로 <see cref="T:Dreamine.Threading.Windows.Services.WindowsTimerResolutionService" /> 클래스의 새 인스턴스를 초기화합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Initializes a new instance of <see cref="T:Dreamine.Threading.Windows.Services.WindowsTimerResolutionService" /> with the default one-millisecond period.</para>
    /// \endif
    /// </summary>
    public WindowsTimerResolutionService()
        : this(1)
    {
    }

    /// <summary>
    /// \if KO
    /// <para>지정한 주기로 <see cref="T:Dreamine.Threading.Windows.Services.WindowsTimerResolutionService" /> 클래스의 새 인스턴스를 초기화합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Initializes a new instance of <see cref="T:Dreamine.Threading.Windows.Services.WindowsTimerResolutionService" /> with the specified period.</para>
    /// \endif
    /// </summary>
    /// <param name="period">
    /// \if KO
    /// <para>밀리초 단위 타이머 해상도 주기이며 0은 1로 보정됩니다.</para>
    /// \endif
    /// \if EN
    /// <para>The timer-resolution period in milliseconds; zero is normalized to one.</para>
    /// \endif
    /// </param>
    public WindowsTimerResolutionService(uint period)
    {
        _period = period == 0 ? 1 : period;
    }

    /// <summary>
    /// \if KO
    /// <para>사용 참조를 증가시키고 첫 참조에서 Windows 고정밀 타이머 해상도를 요청합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Increments the usage reference and requests high-precision Windows timer resolution on the first reference.</para>
    /// \endif
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// \if KO
    /// <para>서비스가 이미 정리된 경우 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when the service has already been disposed.</para>
    /// \endif
    /// </exception>
    /// <exception cref="DllNotFoundException">
    /// \if KO
    /// <para><c>winmm.dll</c>을 로드할 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when <c>winmm.dll</c> cannot be loaded.</para>
    /// \endif
    /// </exception>
    /// <remarks>
    /// \if KO
    /// <para>네이티브 반환 코드는 현재 구현에서 검사하지 않습니다.</para>
    /// \endif
    /// \if EN
    /// <para>The current implementation does not inspect the native return code.</para>
    /// \endif
    /// </remarks>
    public void Begin()
    {
        lock (_syncRoot)
        {
            ThrowIfDisposed();

            if (_referenceCount == 0)
            {
                WinMmNativeMethods.timeBeginPeriod(_period);
            }

            _referenceCount++;
        }
    }

    /// <summary>
    /// \if KO
    /// <para>사용 참조를 감소시키고 마지막 참조에서 Windows 타이머 해상도 요청을 해제합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Decrements the usage reference and clears the Windows timer-resolution request on the final reference.</para>
    /// \endif
    /// </summary>
    /// <exception cref="DllNotFoundException">
    /// \if KO
    /// <para><c>winmm.dll</c>을 로드할 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when <c>winmm.dll</c> cannot be loaded.</para>
    /// \endif
    /// </exception>
    /// <remarks>
    /// \if KO
    /// <para>정리된 상태이거나 활성 참조가 없으면 아무 작업도 하지 않습니다.</para>
    /// \endif
    /// \if EN
    /// <para>No action is taken after disposal or when no active reference exists.</para>
    /// \endif
    /// </remarks>
    public void End()
    {
        lock (_syncRoot)
        {
            if (_disposed)
            {
                return;
            }

            if (_referenceCount <= 0)
            {
                return;
            }

            _referenceCount--;

            if (_referenceCount == 0)
            {
                WinMmNativeMethods.timeEndPeriod(_period);
            }
        }
    }

    /// <summary>
    /// \if KO
    /// <para>남은 모든 타이머 해상도 참조를 해제하고 서비스를 정리합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Releases all remaining timer-resolution references and disposes the service.</para>
    /// \endif
    /// </summary>
    /// <exception cref="DllNotFoundException">
    /// \if KO
    /// <para>활성 참조를 해제하는 동안 <c>winmm.dll</c>을 로드할 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when <c>winmm.dll</c> cannot be loaded while clearing active references.</para>
    /// \endif
    /// </exception>
    public void Dispose()
    {
        lock (_syncRoot)
        {
            if (_disposed)
            {
                return;
            }

            while (_referenceCount > 0)
            {
                WinMmNativeMethods.timeEndPeriod(_period);
                _referenceCount--;
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// \if KO
    /// <para>타이머 해상도 서비스가 아직 정리되지 않았는지 확인합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Verifies that the timer-resolution service has not been disposed.</para>
    /// \endif
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// \if KO
    /// <para>서비스가 이미 정리된 경우 발생합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Thrown when the service has already been disposed.</para>
    /// \endif
    /// </exception>
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(WindowsTimerResolutionService));
        }
    }
}
