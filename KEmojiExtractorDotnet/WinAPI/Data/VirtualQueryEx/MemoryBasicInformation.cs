using System;
using System.Runtime.InteropServices;

namespace KEmojiExtractorDotnet.WinAPI.Data.VirtualQueryEx
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBasicInformation
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public IntPtr RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MemoryBasicInformation.Type"/>
    [Flags]
    public enum MemType : uint
    {
        /// <summary> 영역 내의 메모리 페이지가 이미지 섹션 보기에 매핑됨을 나타냅니다. </summary>
        MEM_IMAGE = 0x1000000,

        /// <summary> 영역 내의 메모리 페이지가 섹션 보기로 매핑됨을 나타냅니다. </summary>
        MEM_MAPPED = 0x40000,

        /// <summary> 영역 내의 메모리 페이지가 전용(즉, 다른 프로세스에서 공유되지 않음)임을 나타냅니다. </summary>
        MEM_PRIVATE = 0x20000
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MemoryBasicInformation.State"/>
    [Flags]
    public enum MemState : uint
    {
        /// <summary> 메모리 또는 디스크의 페이징 파일에서 물리적 저장소가 할당된 커밋된 페이지를 나타냅니다. </summary>
        MEM_COMMIT = 0x00001000,

        /// <summary> 호출 프로세스에 액세스할 수 없고 할당할 수 있는 사용 가능한 페이지를 나타냅니다. 여유 페이지의 경우 AllocationBase , AllocationProtect , Protect 및 Type 멤버 의 정보는 정의되지 않습니다. </summary>
        MEM_FREE = 0x00010000,

        /// <summary> 물리적 저장소가 할당되지 않고 프로세스의 가상 주소 공간 범위가 예약된 예약된 페이지를 나타냅니다. 예약된 페이지의 경우 Protect 구성원 의 정보는 정의되지 않습니다. </summary>
        MEM_RESERVE = 0x00020000
    }
}
