using Dreamine.Threading.Windows.Services;

namespace Dreamine.Threading.Windows.Tests;

public sealed class WindowsProcessCpuUsageProviderTests
{
    [Fact]
    public void CpuUsage_IsClampedToPercentageRange()
    {
        using var provider = new WindowsProcessCpuUsageProvider();

        var usage = provider.GetTotalCpuUsagePercent();

        Assert.InRange(usage, 0, 100);
    }

    [Fact]
    public void Dispose_IsIdempotentAndRetainsLastReading()
    {
        var provider = new WindowsProcessCpuUsageProvider();
        var beforeDispose = provider.GetTotalCpuUsagePercent();

        provider.Dispose();
        provider.Dispose();

        Assert.Equal(beforeDispose, provider.GetTotalCpuUsagePercent());
    }
}
