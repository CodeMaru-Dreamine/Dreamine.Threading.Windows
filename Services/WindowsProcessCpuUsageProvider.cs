using System;
using System.Diagnostics;
using Dreamine.Threading.Interfaces;

namespace Dreamine.Threading.Windows.Services;

/// <summary>
/// \if KO
/// <para>Windows에서 현재 프로세스의 CPU 사용률 정보를 제공합니다.</para>
/// \endif
/// \if EN
/// <para>Provides CPU-usage information for the current process on Windows.</para>
/// \endif
/// </summary>
public sealed class WindowsProcessCpuUsageProvider : ICpuUsageProvider, IDisposable
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
    /// <para>process 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the process value.</para>
    /// \endif
    /// </summary>
    private readonly Process _process;
    /// <summary>
    /// \if KO
    /// <para>last Processor Time 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the last processor time value.</para>
    /// \endif
    /// </summary>
    private TimeSpan _lastProcessorTime;
    /// <summary>
    /// \if KO
    /// <para>last Timestamp 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the last timestamp value.</para>
    /// \endif
    /// </summary>
    private DateTimeOffset _lastTimestamp;
    /// <summary>
    /// \if KO
    /// <para>last Cpu Usage 값을 보관합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Stores the last cpu usage value.</para>
    /// \endif
    /// </summary>
    private double _lastCpuUsage;
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
    /// <para>현재 프로세스의 초기 CPU 시간과 시각으로 <see cref="T:Dreamine.Threading.Windows.Services.WindowsProcessCpuUsageProvider" /> 클래스의 새 인스턴스를 초기화합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Initializes a new instance of <see cref="T:Dreamine.Threading.Windows.Services.WindowsProcessCpuUsageProvider" /> from the current process's initial CPU time and timestamp.</para>
    /// \endif
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// \if KO
    /// <para>현재 프로세스 정보를 가져올 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when current-process information cannot be obtained.</para>
    /// \endif
    /// </exception>
    public WindowsProcessCpuUsageProvider()
    {
        _process = Process.GetCurrentProcess();
        _lastProcessorTime = _process.TotalProcessorTime;
        _lastTimestamp = DateTimeOffset.Now;
    }

    /// <summary>
    /// \if KO
    /// <para>최소 200밀리초 표본 간격으로 현재 프로세스의 코어 정규화 CPU 사용률을 계산합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Calculates core-normalized CPU usage for the current process using a minimum 200-millisecond sampling interval.</para>
    /// \endif
    /// </summary>
    /// <returns>
    /// \if KO
    /// <para>0~100으로 제한된 CPU 사용률이며 너무 이른 호출이나 정리 후에는 마지막 값을 반환합니다.</para>
    /// \endif
    /// \if EN
    /// <para>CPU usage clamped to zero through 100; the last value is returned for early samples or after disposal.</para>
    /// \endif
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// \if KO
    /// <para>프로세스 성능 정보를 새로 고치거나 읽을 수 없을 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when process performance information cannot be refreshed or read.</para>
    /// \endif
    /// </exception>
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
    /// \if KO
    /// <para>내부 <see cref="T:System.Diagnostics.Process" /> 핸들을 해제하며 정리 오류는 억제합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Releases the underlying <see cref="T:System.Diagnostics.Process" /> handle and suppresses cleanup errors.</para>
    /// \endif
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
