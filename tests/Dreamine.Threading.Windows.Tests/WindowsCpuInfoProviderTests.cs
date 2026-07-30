using Dreamine.Threading.Windows.Services;

namespace Dreamine.Threading.Windows.Tests;

public sealed class WindowsCpuInfoProviderTests
{
    [Fact]
    public void LogicalProcessorCount_IsAlwaysPositive()
    {
        var provider = new WindowsCpuInfoProvider();

        Assert.True(provider.GetLogicalProcessorCount() >= 1);
    }

    [Fact]
    public void CoreValidation_AcceptsFirstAndRejectsBoundaries()
    {
        var provider = new WindowsCpuInfoProvider();
        var count = provider.GetLogicalProcessorCount();

        Assert.True(provider.IsValidCoreIndex(0));
        Assert.False(provider.IsValidCoreIndex(-1));
        Assert.False(provider.IsValidCoreIndex(count));
    }
}
