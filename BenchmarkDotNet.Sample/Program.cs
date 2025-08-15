using System;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace BenchmarkDotNetSample
{
    public static class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<Test>(
                DefaultConfig.Instance
                    .WithSummaryStyle(SummaryStyle.Default.WithRatioStyle(RatioStyle.Trend))
            );
        }
    }

    public partial class Test
    {
        private const int OPERATIONS_PER_INVOKE = 1;
    
        static readonly Args args = new Args();
        public object[] GetArgs() => new[] { args };

        [Benchmark(Baseline = true, OperationsPerInvoke = OPERATIONS_PER_INVOKE)]
        [ArgumentsSource(nameof(GetArgs))]
        public byte[] NaiveCopy(Args args)
        {
            byte[] result = new byte[args.height * args.stride];

            for (int y = 0; y < args.height; y++)
            for (int x = 0; x < args.width; x++)
            for (int i = 0; i < args.pattern.Length; i++)
                result[y * args.stride + x * args.pattern.Length + i] = args.pattern[i];

            return result;
        }

        [Benchmark(OperationsPerInvoke = OPERATIONS_PER_INVOKE)]
        [ArgumentsSource(nameof(GetArgs))]
        public byte[] ArrayCopy(Args args)
        {
            byte[] result = new byte[args.height * args.stride];

            Array.Copy(args.pattern, result, args.pattern.Length);
            int numBytesWritten = args.pattern.Length;

            for (; numBytesWritten * 2 < result.Length; numBytesWritten <<= 1)
                Array.Copy(result, 0, result, numBytesWritten, numBytesWritten);

            int remainder = result.Length - numBytesWritten;
            Array.Copy(result, 0, result, numBytesWritten, remainder);

            return result;
        }

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr source, int count);

        [Benchmark(OperationsPerInvoke = OPERATIONS_PER_INVOKE)]
        [ArgumentsSource(nameof(GetArgs))]
        public byte[] MemCopy(Args args)
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

#if NET7_0_OR_GREATER
        [LibraryImport("msvcrt.dll", EntryPoint = "memcpy", SetLastError = false)]
        public static partial IntPtr memcpy_libimport(IntPtr dest, IntPtr source, int count);

        [Benchmark(OperationsPerInvoke = OPERATIONS_PER_INVOKE)]
        [ArgumentsSource(nameof(GetArgs))]
        public byte[] MemCopyLibImport(Args args)
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
#endif
    }
}