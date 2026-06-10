using Dreamine.Threading.Interfaces;

namespace Dreamine.Threading.Windows.Services;

/// <summary>
/// Provides CPU information for Windows threading services.
/// </summary>
public sealed class WindowsCpuInfoProvider : ICpuInfoProvider
{
    /// <inheritdoc />
    public int GetLogicalProcessorCount()
    {
        return Environment.ProcessorCount <= 0
            ? 1
            : Environment.ProcessorCount;
    }

    /// <inheritdoc />
    public bool IsValidCoreIndex(int coreIndex)
    {
        return coreIndex >= 0 && coreIndex < GetLogicalProcessorCount();
    }
}
