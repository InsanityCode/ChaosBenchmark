using System;
using System.Runtime.InteropServices;

namespace Sample.FillBuffer.Tests
{
    static class MemCopy
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr source, int count);

        public static byte[] Perform(Args args)
        {
            byte[] result = new byte[args.height * args.stride];

            Array.Copy(args.pattern, result, args.pattern.Length);
            int numBytesWritten = args.pattern.Length;

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
