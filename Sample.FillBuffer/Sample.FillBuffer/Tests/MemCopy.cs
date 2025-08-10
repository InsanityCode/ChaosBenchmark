using System;
using System.Runtime.InteropServices;

namespace Sample.FillBuffer.Tests
{
    using static Program;

    static class MemCopy
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr source, int count);

        public static byte[] Perform()
        {
            byte[] result = new byte[HEIGHT * stride];

            Array.Copy(pattern, result, pattern.Length);
            int numBytesWritten = pattern.Length;

            GCHandle resultHandle = GCHandle.Alloc(result);
            IntPtr src = Marshal.UnsafeAddrOfPinnedArrayElement(result, 0);

            for (; numBytesWritten * 2 < result.Length; numBytesWritten <<= 1)
                memcpy(Marshal.UnsafeAddrOfPinnedArrayElement(result, numBytesWritten), src, numBytesWritten);

            int remainder = result.Length - numBytesWritten;
            memcpy(Marshal.UnsafeAddrOfPinnedArrayElement(result, numBytesWritten), src, remainder);

            resultHandle.Free();

            return result;
        }
    }
}
