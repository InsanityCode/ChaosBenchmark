using ChaosBenchmark;

#if NET7_0_OR_GREATER
using System;
using System.Runtime.InteropServices;
#endif

namespace Sample.FillBuffer.Tests.MemCopy
{
    partial class LibImport : Test<byte[], Args>
    {
#if NET7_0_OR_GREATER
        [LibraryImport("msvcrt.dll", EntryPoint = "memcpy", SetLastError = false)]
        public static partial IntPtr memcpy(IntPtr dest, IntPtr source, int count);
#endif

        public override string Name() => $"{nameof(MemCopy)}.{nameof(LibImport)}";

        public override bool Supported()
        =>
#if NET7_0_OR_GREATER
            true;
#else
            false;
#endif

        public override byte[] Invoke(Args args)
        {
#if NET7_0_OR_GREATER
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
#else
            return null;
#endif
        }
    }
}
