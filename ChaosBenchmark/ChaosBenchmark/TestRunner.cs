using System;

namespace ChaosBenchmark
{
    public static class TestRunner
    {
        static readonly Random random = new Random();

        public static void Run<Result>(
            Test<Result>[] tests,
            int numBatches,
            int batchSize,
            int printStep
            )
        {
            string batchFmt = "D" + numBatches.ToString().Length;

            int padding = 0;
            foreach (Test<Result> test in tests)
                padding = Math.Max(padding, test.name.Length);

            for (int i = 0; i < numBatches; i++)
            {
                if (i % printStep == 0)
                    Print(i, numBatches, batchFmt);

                System.Collections.Generic.List<Test<Result>> batchTests
                    = new System.Collections.Generic.List<Test<Result>>(tests);

                while (batchTests.Count > 0)
                {
                    int rnd = random.Next(batchTests.Count);
                    Test<Result> chosen = batchTests[rnd];
                    batchTests.RemoveAt(rnd);
                    chosen.Run(batchSize);
                }
            }
            Print(numBatches, numBatches, batchFmt);

            Console.WriteLine();
            foreach (Test<Result> test in tests)
                test.PrintResult(padding);
        }

        static void Print(int i, int numBatches, string batchFmt)
            => Console.Write($"\rRunning: {i.ToString(batchFmt)}/{numBatches} = {(100.0 * i / numBatches).ToString("000.0")}%");
    }
}
