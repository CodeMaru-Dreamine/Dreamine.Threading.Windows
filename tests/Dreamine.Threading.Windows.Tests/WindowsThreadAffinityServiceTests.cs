using Dreamine.Threading.Windows.Services;

namespace Dreamine.Threading.Windows.Tests;

public sealed class WindowsThreadAffinityServiceTests
{
    [Fact]
    public void NegativeCoreIndex_IsRejectedBeforeNativeCall()
    {
        var service = new WindowsThreadAffinityService();

        Assert.Throws<ArgumentOutOfRangeException>(() => service.ApplyToCurrentThread(-1));
    }

    [Fact]
    public void CoreIndexBeyondPointerMask_IsRejectedBeforeNativeCall()
    {
        var service = new WindowsThreadAffinityService();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => service.ApplyToCurrentThread(IntPtr.Size * 8));
    }

    [Fact]
    public void ClearWithoutRestoreToken_IsSafe()
    {
        var service = new WindowsThreadAffinityService();

        service.ClearCurrentThreadAffinity();
    }
}
