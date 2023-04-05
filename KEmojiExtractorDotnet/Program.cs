using ImageMagick;
using KEmojiExtractorDotnet.WinAPI;
using KEmojiExtractorDotnet.WinAPI.Data.GetSystemInfo;
using KEmojiExtractorDotnet.WinAPI.Data.OpenProcess;
using KEmojiExtractorDotnet.WinAPI.Data.VirtualQueryEx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace KEmojiExtractorDotnet
{
    /// <summary>
    /// https://github.com/rollrat/kemoji-extractor
    /// </summary>
    /// <param name="args"></param>
    internal class Program
    {
        private static readonly List<string> cache = new List<string>();

        static void Main(string[] args)
        {
            //프로세스 정보 가져오기
            Console.Write("프로세스명 입력: ");
            var processName = Console.ReadLine();
            var processID = Process.GetProcessesByName(processName)?.FirstOrDefault()?.Id;
            var processHandle = API.OpenProcess(ProcessAccessFlag.All, false, processID.GetValueOrDefault());
            if (processID == null || processHandle == null)
            {
                return;
            }

            //시스템 정보 가져오기
            SystemInformation systemInfo = new SystemInformation();
            API.GetSystemInfo(ref systemInfo);

            //프로세스 메모리 스캔 범위 초기화
            var minAddress = systemInfo.MinimumApplicationAddress;
            var maxAddress = systemInfo.MaximumApplicationAddress;
            var currentAddress = minAddress;
            var memoryBasicInformationSize = (uint)Marshal.SizeOf(typeof(MemoryBasicInformation));

            //프로세스 메모리 범위 스캔
            while (true)
            {
                //프로세스 메모리 정보 읽기
                var resultSize = API.VirtualQueryEx(processHandle, currentAddress, out MemoryBasicInformation memoryBasicInformation, memoryBasicInformationSize);
                if (resultSize != memoryBasicInformationSize)
                {
                    currentAddress = minAddress;
                    continue;
                }

                var type = memoryBasicInformation.Type;
                var state = memoryBasicInformation.State;
                var baseAddres = memoryBasicInformation.BaseAddress;
                var regionSize = memoryBasicInformation.RegionSize;

                //읽은 정보가 조건에 만족하면 메모리 체크
                if (type == (uint)MemType.MEM_PRIVATE && state == (uint)MemState.MEM_COMMIT && regionSize != IntPtr.Zero)
                {
                    //버퍼 할당
                    var buffer = Marshal.AllocHGlobal(regionSize);

                    //메모리 프로텍트 변경
                    var protectChanged = API.VirtualProtectEx(processHandle, baseAddres, (UIntPtr)API.MEMORY_BUFFER_SIZE, API.PAGE_EXECUTE_READWRITE, out uint old);

                    //메모리 읽기
                    if (API.ReadProcessMemory(processHandle, baseAddres, buffer, (uint)regionSize, out _))
                    {
                        //16: RIFF(4) + SIZE(4) + WEBP(4) + DATA(4~)
                        for (var i = 0; i < ((long)regionSize - 16); i++)
                        {
                            if ((i + API.MEMORY_BUFFER_SIZE + 1) > (long)regionSize)
                            {
                                break;
                            }

                            var riffMagicNumber = Marshal.ReadInt32(buffer, i);
                            var fileSize = Marshal.ReadInt32(buffer, i + 4);
                            var webpMagicNumber = Marshal.ReadInt32(buffer, i + 8);

                            if (riffMagicNumber == API.RIFF_MAGIC_NUMBER && webpMagicNumber == API.WEBP_MAGIC_NUMBER && fileSize != 0)
                            {
                                var basePath = "emoji";
                                var webpFileName = Path.Combine(basePath, $"mang_{baseAddres + i}.webp");
                                var gifFileName = Path.Combine(basePath, $"mang_{baseAddres + i}.gif");

                                //이미 처리했으면 스킵
                                if (cache.Contains(webpFileName))
                                {
                                    continue;
                                }

                                //폴더 생성
                                System.IO.Directory.CreateDirectory(basePath);

                                //webp 추출
                                using (FileStream file = new FileStream(webpFileName, FileMode.Create, FileAccess.Write))
                                {
                                    var saved = API.WriteFile(file.SafeFileHandle, buffer + i, fileSize + 12, out int lpNumberOfBytesWritten, IntPtr.Zero);
                                    cache.Add(webpFileName);
                                    Console.WriteLine($"{webpFileName}: {saved}");
                                }

                                //gif 변환
                                WebpToGif(webpFileName, gifFileName, width: 128, height: 128, deleteOriginFile: true);
                            }
                        }
                    }
                    else
                    {
                        //메모리 프로텍트 복원
                        if (protectChanged)
                        {
                            API.VirtualProtectEx(processHandle, baseAddres, (UIntPtr)API.MEMORY_BUFFER_SIZE, old, out _);
                        }
                    }

                    //버퍼 초기화
                    Marshal.FreeHGlobal(buffer);

                }

                var newAddress = (ulong)baseAddres + (ulong)regionSize;
                if (newAddress > (ulong)maxAddress)
                {
                    currentAddress = minAddress;
                    continue;
                }

                currentAddress = new UIntPtr(newAddress);
            }
        }

        /// <summary>
        /// Webp -> Gif
        /// </summary>
        /// <param name="webpFileName"></param>
        /// <param name="gifFileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="deleteOriginFile"></param>
        private static void WebpToGif(string webpFileName, string gifFileName, int width = 128, int height = 128, bool deleteOriginFile = true)
        {
            //gif 변환
            try
            {
                using (var animatedWebP = new MagickImageCollection(webpFileName))
                {
                    //loop
                    animatedWebP[0].AnimationIterations = 0;

                    //resize
                    if (width >= 0 && height >= 0)
                    {
                        foreach (var img in animatedWebP)
                        {
                            img.Resize(width, height);
                        }
                    }

                    animatedWebP.Write(gifFileName, MagickFormat.Gif);
                }
            }
            catch (Exception ex)
            {
                Console.Write($"{gifFileName} ERROR");
            }

            //기존 파일 삭제
            if (deleteOriginFile)
            {
                File.Delete(webpFileName);
            }
        }
    }
}
