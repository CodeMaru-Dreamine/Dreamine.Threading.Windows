using Dreamine.Threading.Windows.Services;

namespace Dreamine.Threading.Windows.Tests;

public sealed class WindowsTimerResolutionServiceTests
{
    [Fact]
    public void BeginEnd_SupportsNestedRequests()
    {
        using var service = new WindowsTimerResolutionService(0);

        service.Begin();
        service.Begin();
        service.End();
        service.End();
        service.End();
    }

    [Fact]
    public void Dispose_ReleasesOutstandingRequestsAndIsIdempotent()
    {
        var service = new WindowsTimerResolutionService();
        service.Begin();

        service.Dispose();
        service.Dispose();

        Assert.Throws<ObjectDisposedException>(service.Begin);
        service.End();
    }
}
