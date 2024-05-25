using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace EldenRingDiscordPresence
{
    internal class MemoryUtility
    {
        private const int PROCESS_VM_READ = 0x0010;
        private const int PROCESS_QUERY_INFORMATION = 0x0400;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const string Pattern = "48 8B 05 ?? ?? ?? ?? 80 B8 ?? ?? ?? ?? 0D 0F 94 C0 C3";
        private static readonly byte[] PatternBytes = ParsePattern(Pattern);
        private static readonly string Mask = CreateMask(Pattern);
        private const int PatternOffset = 3;
        private const int AddressAdjustment = 7;
        private const long GraceOffset = 0xB3C;

        private IntPtr _processHandle = IntPtr.Zero;
        private Process _process;
        private ProcessModule _processModule;
        private long _mainAddress = 0;

        public void OpenMemoryProcess()
        {
            _process = Process.GetProcessesByName(Program.CurrentGameExecutable).FirstOrDefault()
                       ?? throw new Exception("Process not found.");
            _processModule = _process.MainModule ?? throw new Exception("Could not find MainModule.");

            _processHandle = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, false, _process.Id);
            if (_processHandle == IntPtr.Zero)
                throw new Exception("Could not open process.");

            _mainAddress = CalculateMainAddress();
        }

        public long ReadLastGraceLocationId()
        {
            if (_mainAddress == 0)
                return 0;

            long mainAddressValue = ReadInt64(_mainAddress);
            return ReadInt32(mainAddressValue + GraceOffset);
        }

        public void CloseMemoryProcess()
        {
            if (_processHandle != IntPtr.Zero)
            {
                CloseHandle(_processHandle);
                _processHandle = IntPtr.Zero;
            }
            _mainAddress = 0;
        }

        private int ReadInt32(long address) => ReadMemory<int>(address, 4, BitConverter.ToInt32);

        private long ReadInt64(long address) => ReadMemory<long>(address, 8, BitConverter.ToInt64);

        private byte[] ReadBytes(long address, int size)
        {
            byte[] buffer = new byte[size];
            if (ReadProcessMemory(_processHandle, (IntPtr)address, buffer, size, out int bytesRead) && bytesRead == size)
            {
                return buffer;
            }
            throw new Exception("Failed to read memory.");
        }

        private T ReadMemory<T>(long address, int size, Func<byte[], int, T> converter)
        {
            byte[] buffer = new byte[size];
            if (ReadProcessMemory(_processHandle, (IntPtr)address, buffer, size, out int bytesRead) && bytesRead == size)
            {
                return converter(buffer, 0);
            }
            return converter(new byte[size], 0);
        }

        private IntPtr CalculateMainAddress()
        {
            IntPtr moduleBase = _processModule.BaseAddress;

            byte[] moduleMemory = ReadBytes(moduleBase.ToInt64(), _processModule.ModuleMemorySize);

            IntPtr patternAddress = FindPattern(moduleMemory, PatternBytes, Mask);
            if (patternAddress == IntPtr.Zero)
                throw new Exception("Pattern not found");

            int offset = BitConverter.ToInt32(moduleMemory, patternAddress.ToInt32() + PatternOffset);
            IntPtr finalAddress = moduleBase + patternAddress.ToInt32() + offset + AddressAdjustment;

            return finalAddress;
        }

        private static IntPtr FindPattern(byte[] moduleMemory, byte[] pattern, string mask)
        {
            for (int i = 0; i <= moduleMemory.Length - pattern.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (mask[j] != '?' && moduleMemory[i + j] != pattern[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return new IntPtr(i);
                }
            }
            return IntPtr.Zero;
        }

        private static byte[] ParsePattern(string pattern)
        {
            return pattern.Split(' ')
                          .Select(x => x == "??" ? (byte)0x00 : Convert.ToByte(x, 16))
                          .ToArray();
        }

        private static string CreateMask(string pattern)
        {
            return new string(pattern.Split(' ')
                                     .Select(x => x == "??" ? '?' : 'x')
                                     .ToArray());
        }
    }
}
