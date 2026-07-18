using Dreamine.Threading.Interfaces;

namespace Dreamine.Threading.Windows.Services;

/// <summary>
/// \if KO
/// <para>Windows 스레딩 서비스에 CPU 정보를 제공합니다.</para>
/// \endif
/// \if EN
/// <para>Provides CPU information for Windows threading services.</para>
/// \endif
/// </summary>
public sealed class WindowsCpuInfoProvider : ICpuInfoProvider
{
    /// <summary>
    /// \if KO
    /// <para>환경의 논리 프로세서 수를 가져오며 비정상 값은 1로 보정합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets the environment's logical processor count, normalizing invalid values to one.</para>
    /// \endif
    /// </summary>
    /// <returns>
    /// \if KO
    /// <para>1 이상의 논리 프로세서 수입니다.</para>
    /// \endif
    /// \if EN
    /// <para>A logical processor count of at least one.</para>
    /// \endif
    /// </returns>
    public int GetLogicalProcessorCount()
    {
        return Environment.ProcessorCount <= 0
            ? 1
            : Environment.ProcessorCount;
    }

    /// <summary>
    /// \if KO
    /// <para>코어 인덱스가 현재 논리 프로세서 범위에 속하는지 확인합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Determines whether a core index is within the current logical-processor range.</para>
    /// \endif
    /// </summary>
    /// <param name="coreIndex">
    /// \if KO
    /// <para>검사할 0부터 시작하는 코어 인덱스입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The zero-based core index to validate.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>유효하면 <see langword="true"/>입니다.</para>
    /// \endif
    /// \if EN
    /// <para><see langword="true"/> when the index is valid.</para>
    /// \endif
    /// </returns>
    public bool IsValidCoreIndex(int coreIndex)
    {
        return coreIndex >= 0 && coreIndex < GetLogicalProcessorCount();
    }
}
