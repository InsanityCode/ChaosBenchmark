using ChaosBenchmark;

namespace Sample.FillBuffer.Tests
{
    class NaiveCopy : Test<byte[], Args>
    {
        public override string Name() => nameof(NaiveCopy);

        public override bool Supported() => true;

        public override byte[] Invoke(Args args)
        {
            byte[] result = new byte[args.height * args.stride];

            for (int y = 0; y < args.height; y++)
                for (int x = 0; x < args.width; x++)
                    for (int i = 0; i < args.pattern.Length; i++)
                        result[y * args.stride + x * args.pattern.Length + i] = args.pattern[i];

            return result;
        }
    }
}
