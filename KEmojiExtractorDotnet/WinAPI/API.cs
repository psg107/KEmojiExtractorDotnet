using KEmojiExtractorDotnet.WinAPI.Data.GetSystemInfo;
using KEmojiExtractorDotnet.WinAPI.Data.OpenProcess;
using KEmojiExtractorDotnet.WinAPI.Data.VirtualQueryEx;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace KEmojiExtractorDotnet.WinAPI
{
    public class API
    {
        #region 임시

        /// <summary>
        /// 0x52 0x49 0x46 0x46 (R I F F, offset: 0)
        /// </summary>
        public const int RIFF_MAGIC_NUMBER = 0x46464952;

        /// <summary>
        ///0x57 0x45 0x42 0x50 (W E B P, offset: 8)
        /// </summary>
        public const int WEBP_MAGIC_NUMBER = 0x50424557;

        /// <summary>
        /// 32768
        /// </summary>
        public static int MEMORY_BUFFER_SIZE = 0x8000;

        public static uint PAGE_EXECUTE_READWRITE = 0x40; 

        #endregion

        /// <summary>
        /// 프로세스 열기
        /// </summary>
        /// <param name="processAccessFlag">프로세스 액세스 플래그</param>
        /// <param name="inheritHandle">핸들 상속 여부</param>
        /// <param name="processID">프로세스 ID</param>
        /// <returns>프로세스 핸들</returns>
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlag processAccessFlag, bool inheritHandle, int processID);

        /// <summary>
        /// 시스템 정보 구하기
        /// </summary>
        /// <param name="systemInformation">시스템 정보</param>
        [DllImport("kernel32")]
        public static extern void GetSystemInfo(ref SystemInformation systemInformation);

        /// <summary>
        /// 지정된 프로세스의 가상 주소 공간 내의 페이지 범위에 대한 정보를 검색합니다.
        /// </summary>
        /// <param name="hProcess">프로세스 핸들</param>
        /// <param name="lpAddress">주소</param>
        /// <param name="lpBuffer">반환 버퍼</param>
        /// <param name="dwLength">크기</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, UIntPtr lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hProcess">메모리 보호를 변경할 프로세스에 대한 핸들입니다.</param>
        /// <param name="lpAddress">액세스 보호 특성을 변경해야 하는 페이지 영역의 기본 주소에 대한 포인터입니다.</param>
        /// <param name="dwSize">액세스 보호 특성이 변경된 지역의 크기(바이트)입니다.</param>
        /// <param name="flNewProtect">메모리 보호 옵션입니다.</param>
        /// <param name="lpflOldProtect">지정한 페이지 영역에 있는 첫 번째 페이지의 이전 액세스 보호를 받는 변수에 대한 포인터입니다.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hProcess">읽는 메모리가 있는 프로세스에 대한 핸들입니다.</param>
        /// <param name="lpBaseAddress">읽을 지정된 프로세스의 기본 주소에 대한 포인터입니다.</param>
        /// <param name="lpBuffer">지정된 프로세스의 주소 공간에서 콘텐츠를 받는 버퍼에 대한 포인터입니다.</param>
        /// <param name="nSize">지정된 프로세스에서 읽을 바이트 수입니다.</param>
        /// <param name="lpNumberOfBytesRead">지정된 버퍼로 전송되는 바이트 수를 받는 변수에 대한 포인터입니다.</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, out uint lpNumberOfBytesRead);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hFile">파일 또는 I/O 장치에 대한 핸들입니다.</param>
        /// <param name="lpBuffer">파일이나 장치에 쓸 데이터가 들어 있는 버퍼에 대한 포인터입니다.</param>
        /// <param name="NumberOfBytesToWrite">파일 또는 장치에 쓸 바이트 수입니다.</param>
        /// <param name="lpNumberOfBytesWritten">동기식 hFile 매개변수를 사용할 때 작성된 바이트 수를 수신하는 변수에 대한 포인터입니다</param>
        /// <param name="lpOverlapped">hFile 매개변수가 FILE_FLAG_OVERLAPPED 로 열린 경우 OVERLAPPED 구조 에 대한 포인터가 필요합니다 . 그렇지 않으면 이 매개변수는 NULL 이 될 수 있습니다 .</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool WriteFile(SafeFileHandle hFile, IntPtr lpBuffer, int NumberOfBytesToWrite, out int lpNumberOfBytesWritten, IntPtr lpOverlapped);
    }
}
