using Dreamine.MVVM.Core;
using Dreamine.Threading.Interfaces;
using Dreamine.Threading.Windows.Services;

namespace Dreamine.Threading.Windows.Registration;

/// <summary>
/// \if KO
/// <para>Windows 전용 Dreamine 스레딩 서비스 등록 API를 제공합니다.</para>
/// \endif
/// \if EN
/// <para>Provides registration APIs for Windows-specific Dreamine threading services.</para>
/// \endif
/// </summary>
public static class DreamineThreadingWindowsRegistration
{
    /// <summary>
    /// \if KO
    /// <para>아직 등록되지 않은 Windows CPU·선호도·타이머 서비스를 전역 컨테이너에 등록합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Registers Windows CPU, affinity, and timer services in the global container when absent.</para>
    /// \endif
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// \if KO
    /// <para>컨테이너 서비스 등록 또는 확인이 실패할 때 발생할 수 있습니다.</para>
    /// \endif
    /// \if EN
    /// <para>May be thrown when container service registration or resolution fails.</para>
    /// \endif
    /// </exception>
    public static void Register()
    {
        if (!DMContainer.IsRegistered<WindowsCpuInfoProvider>())
        {
            DMContainer.RegisterSingleton<WindowsCpuInfoProvider>();
        }

        if (!DMContainer.IsRegistered<ICpuInfoProvider>())
        {
            DMContainer.RegisterSingleton<ICpuInfoProvider>(
                DMContainer.Resolve<WindowsCpuInfoProvider>());
        }

        if (!DMContainer.IsRegistered<IThreadAffinityService>())
        {
            DMContainer.RegisterSingleton<IThreadAffinityService, WindowsThreadAffinityService>();
        }

        if (!DMContainer.IsRegistered<ITimerResolutionService>())
        {
            DMContainer.RegisterSingleton<ITimerResolutionService>(
                new WindowsTimerResolutionService());
        }

        if (!DMContainer.IsRegistered<ICpuUsageProvider>())
        {
            DMContainer.RegisterSingleton<ICpuUsageProvider>(
                new WindowsProcessCpuUsageProvider());
        }
    }
}
