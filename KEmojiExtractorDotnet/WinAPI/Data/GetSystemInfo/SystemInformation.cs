using System;
using System.Runtime.InteropServices;

namespace KEmojiExtractorDotnet.WinAPI.Data.GetSystemInfo
{
    /// <summary>
    /// 시스템 정보
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemInformation
    {
        /// <summary>
        /// 프로세서 아키텍처
        /// </summary>
        public ProcessorArchitecture ProcessorArchitecture;

        /// <summary>
        /// 예약
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// 페이지 크기
        /// </summary>
        public uint PageSize;

        /// <summary>
        /// 최소 애플리케이션 주소
        /// </summary>
        public UIntPtr MinimumApplicationAddress;

        /// <summary>
        /// 최대 애플리케이션 주소
        /// </summary>
        public UIntPtr MaximumApplicationAddress;

        /// <summary>
        /// 활성 프로세서 마스크
        /// </summary>
        public UIntPtr ActiveProcessorMask;

        /// <summary>
        /// 프로세서 카운트
        /// </summary>
        public uint ProcessorCount;

        /// <summary>
        /// 프로세서 타입
        /// </summary>
        public uint ProcessorType;

        /// <summary>
        /// 할당 입도
        /// </summary>
        public uint AllocationGranularity;

        /// <summary>
        /// 프로세서 레벨
        /// </summary>
        public ushort ProcessorLevel;

        /// <summary>
        /// 프로세서 수정
        /// </summary>
        public ushort ProcessorRevision;
    }

    /// <summary>
    /// 프로세서 아키텍처
    /// </summary>
    public enum ProcessorArchitecture : ushort
    {
        /// <summary>
        /// Intel
        /// </summary>
        Intel = 0,

        /// <summary>
        /// IA64
        /// </summary>
        IA64 = 6,

        /// <summary>
        /// AMD64
        /// </summary>
        AMD64 = 9,

        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0xffff
    }
}
