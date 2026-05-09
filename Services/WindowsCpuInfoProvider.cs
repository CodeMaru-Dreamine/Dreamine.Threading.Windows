namespace Dreamine.Threading.Windows.Services;

/// <summary>
/// Provides CPU information for Windows threading services.
/// </summary>
public sealed class WindowsCpuInfoProvider
{
    /// <summary>
    /// Gets the logical processor count.
    /// </summary>
    /// <returns>The logical processor count.</returns>
    public int GetLogicalProcessorCount()
    {
        return Environment.ProcessorCount <= 0
            ? 1
            : Environment.ProcessorCount;
    }

    /// <summary>
    /// Determines whether the specified CPU core index is valid.
    /// </summary>
    /// <param name="coreIndex">The CPU core index.</param>
    /// <returns>True if the core index is valid; otherwise false.</returns>
    public bool IsValidCoreIndex(int coreIndex)
    {
        return coreIndex >= 0 && coreIndex < GetLogicalProcessorCount();
    }
}