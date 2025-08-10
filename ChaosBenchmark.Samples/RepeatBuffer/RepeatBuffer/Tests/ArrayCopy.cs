using System;

namespace ChaosBenchmark.Samples.RepeatBuffer.Tests
{
    class ArrayCopy : Test<byte[], Args>
    {
        public override string Name() => nameof(ArrayCopy);

        public override bool Supported() => true;

        public override byte[] Invoke(Args args)
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
    }
}
