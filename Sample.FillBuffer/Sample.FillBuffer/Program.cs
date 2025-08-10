using ChaosBenchmark;

namespace Sample.FillBuffer
{
    static class Program
    {
        internal const int WIDTH = 512;
        internal const int HEIGHT = 512;

        internal static byte[] pattern = [0x01, 0x02, 0x03, 0x04];
        internal static readonly int stride = WIDTH * pattern.Length // required buffer size
                                            + pattern.Length * 8     // some extra space per row
                                            ;

        public static int Main(string[] args)
        {
            Test<byte[]>[] tests = [
                new Test<byte[]>(nameof(Tests.NaiveCopy), Tests.NaiveCopy.Perform)
            ];

            // verfify correct result
            foreach (Test<byte[]> test in tests)
            {
                byte[] result = test.action();
                for (int y = 0; y < HEIGHT; y++)
                    for (int x = 0; x < WIDTH; x++)
                        for (int i = 0; i < pattern.Length; i++)
                            if (result[y * stride + x * pattern.Length + i] != pattern[i])
                                throw new System.Exception($"'{test.name}' yielded wrong result!");
            }

            TestRunner.Run(tests, 100, 10, 10);
            return 0;
        }
    }
}
