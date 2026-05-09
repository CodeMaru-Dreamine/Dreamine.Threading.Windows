using Dreamine.Threading.Interfaces;
using Dreamine.Threading.Windows.Native;

namespace Dreamine.Threading.Windows.Services;

/// <summary>
/// Controls Windows timer resolution for high precision thread cycles.
/// </summary>
public sealed class WindowsTimerResolutionService : ITimerResolutionService, IDisposable
{
    private readonly object _syncRoot = new();
    private readonly uint _period;
    private int _referenceCount;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsTimerResolutionService"/> class.
    /// </summary>
    public WindowsTimerResolutionService()
        : this(1)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsTimerResolutionService"/> class.
    /// </summary>
    /// <param name="period">The timer resolution period in milliseconds.</param>
    public WindowsTimerResolutionService(uint period)
    {
        _period = period == 0 ? 1 : period;
    }

    /// <summary>
    /// Begins high precision timer resolution.
    /// </summary>
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
    /// Ends high precision timer resolution.
    /// </summary>
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
    /// Releases timer resolution requests.
    /// </summary>
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

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(WindowsTimerResolutionService));
        }
    }
}