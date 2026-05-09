using System;
using System.Diagnostics;
using Dreamine.Threading.Interfaces;

namespace Dreamine.Threading.Windows.Services;

/// <summary>
/// Provides process CPU usage information on Windows.
/// </summary>
public sealed class WindowsProcessCpuUsageProvider : ICpuUsageProvider, IDisposable
{
    private readonly object _syncRoot = new();
    private readonly Process _process;
    private TimeSpan _lastProcessorTime;
    private DateTimeOffset _lastTimestamp;
    private double _lastCpuUsage;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsProcessCpuUsageProvider"/> class.
    /// </summary>
    public WindowsProcessCpuUsageProvider()
    {
        _process = Process.GetCurrentProcess();
        _lastProcessorTime = _process.TotalProcessorTime;
        _lastTimestamp = DateTimeOffset.Now;
    }

    /// <inheritdoc />
    public double GetTotalCpuUsagePercent()
    {
        lock (_syncRoot)
        {
            if (_disposed)
            {
                return _lastCpuUsage;
            }

            _process.Refresh();

            var now = DateTimeOffset.Now;
            var currentProcessorTime = _process.TotalProcessorTime;

            var elapsedMs = (now - _lastTimestamp).TotalMilliseconds;
            if (elapsedMs < 200)
            {
                return _lastCpuUsage;
            }

            var cpuTimeMs = (currentProcessorTime - _lastProcessorTime).TotalMilliseconds;

            var cpuUsage = cpuTimeMs / (elapsedMs * Environment.ProcessorCount) * 100.0;

            _lastProcessorTime = currentProcessorTime;
            _lastTimestamp = now;
            _lastCpuUsage = Math.Clamp(cpuUsage, 0, 100);

            return _lastCpuUsage;
        }
    }

    /// <summary>
    /// Releases the underlying <see cref="Process"/> handle.
    /// </summary>
    public void Dispose()
    {
        lock (_syncRoot)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            try
            {
                _process.Dispose();
            }
            catch
            {
                // Suppress on dispose.
            }
        }
    }
}
