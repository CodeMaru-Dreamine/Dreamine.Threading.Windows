using Dreamine.MVVM.Core;
using Dreamine.Threading.Interfaces;
using Dreamine.Threading.Windows.Registration;
using Dreamine.Threading.Windows.Services;

namespace Dreamine.Threading.Windows.Tests;

public sealed class DreamineThreadingWindowsRegistrationTests : IDisposable
{
    public DreamineThreadingWindowsRegistrationTests() => DMContainer.Reset();

    public void Dispose() => DMContainer.Reset();

    [Fact]
    public void Register_adds_all_windows_services_and_is_idempotent()
    {
        DreamineThreadingWindowsRegistration.Register();

        Assert.Same(
            DMContainer.Resolve<WindowsCpuInfoProvider>(),
            DMContainer.Resolve<ICpuInfoProvider>());
        Assert.IsType<WindowsThreadAffinityService>(
            DMContainer.Resolve<IThreadAffinityService>());
        Assert.IsType<WindowsTimerResolutionService>(
            DMContainer.Resolve<ITimerResolutionService>());
        Assert.IsType<WindowsProcessCpuUsageProvider>(
            DMContainer.Resolve<ICpuUsageProvider>());

        DreamineThreadingWindowsRegistration.Register();
    }
}
