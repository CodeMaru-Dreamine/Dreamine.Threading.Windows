using Dreamine.MVVM.Core;
using Dreamine.Threading.Interfaces;
using Dreamine.Threading.Windows.Services;

namespace Dreamine.Threading.Windows.Registration;

/// <summary>
/// Provides registration APIs for Windows-specific Dreamine threading services.
/// </summary>
public static class DreamineThreadingWindowsRegistration
{
    /// <summary>
    /// Registers Windows-specific Dreamine threading services.
    /// </summary>
    public static void Register()
    {
        if (!DMContainer.IsRegistered<WindowsCpuInfoProvider>())
        {
            DMContainer.RegisterSingleton<WindowsCpuInfoProvider>();
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