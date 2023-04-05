using System;

namespace KEmojiExtractorDotnet.WinAPI.Data.OpenProcess
{
    /// <summary>
    /// 프로세스 액세스 플래그
    /// </summary>
    [Flags]
    public enum ProcessAccessFlag : uint
    {
        /// <summary>
        /// All
        /// </summary>
        All = 0x001f0fff,

        /// <summary>
        /// Terminate
        /// </summary>
        Terminate = 0x00000001,

        /// <summary>
        /// CreateThread
        /// </summary>
        CreateThread = 0x00000002,

        /// <summary>
        /// VirtualMemoryOperation
        /// </summary>
        VirtualMemoryOperation = 0x00000008,

        /// <summary>
        /// VirtualMemoryRead
        /// </summary>
        VirtualMemoryRead = 0x00000010,

        /// <summary>
        /// VirtualMemoryWrite
        /// </summary>
        VirtualMemoryWrite = 0x00000020,

        /// <summary>
        /// DuplicateHandle
        /// </summary>
        DuplicateHandle = 0x00000040,

        /// <summary>
        /// CreateProcess
        /// </summary>
        CreateProcess = 0x000000080,

        /// <summary>
        /// SetQuota
        /// </summary>
        SetQuota = 0x00000100,

        /// <summary>
        /// SetInformation
        /// </summary>
        SetInformation = 0x00000200,

        /// <summary>
        /// QueryInformation
        /// </summary>
        QueryInformation = 0x00000400,

        /// <summary>
        /// QueryLimitedInformation
        /// </summary>
        QueryLimitedInformation = 0x00001000,

        /// <summary>
        /// Synchronize
        /// </summary>
        Synchronize = 0x00100000
    }
}
