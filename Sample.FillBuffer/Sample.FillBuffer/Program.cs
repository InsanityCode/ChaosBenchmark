using ChaosBenchmark;

namespace Sample.FillBuffer
{
    static class Program
    {
        static bool Verify(Args args, byte[] result)
        {
            for (int y = 0; y < args.height; y++)
                for (int x = 0; x < args.width; x++)
                    for (int i = 0; i < args.pattern.Length; i++)
                        if (result[y * args.stride + x * args.pattern.Length + i] != args.pattern[i])
                            return false;

            return true;
        }

        public static int Main(string[] args)
        {
            Test<byte[], Args>[] tests = [
                new Test<byte[], Args>(nameof(Tests.NaiveCopy), Tests.NaiveCopy.Perform),
                new Test<byte[], Args>(nameof(Tests.ArrayCopy), Tests.ArrayCopy.Perform),
                new Test<byte[], Args>(nameof(Tests.MemCopy),   Tests.MemCopy.Perform),
            ];

            TestRunner<byte[], Args>.Run(tests, Args.CreateRandom, Verify, 10, 10);
            return 0;
        }
    }
}
