using System;

namespace ChaosBenchmark
{
    public static class TestRunner<Result, Args>
    {
        public delegate Args CreateBatchArgs();
        public delegate bool VerifyResult(Args args, Result result);

        static readonly Random random = new Random();

        public static void Run(
            Test<Result, Args>[] tests,
            CreateBatchArgs createArgs,
            VerifyResult verifyResult,
            int numBatches,
            int batchSize
            )
        {
            string batchFmt = "D" + numBatches.ToString().Length;

            int padding = 0;
            foreach (Test<Result, Args> test in tests)
                padding = Math.Max(padding, test.Name().Length);

            for (int i = 0; i < numBatches; i++)
            {
                Args args = createArgs();

                Print(i, numBatches, batchFmt);

                System.Collections.Generic.List<Test<Result, Args>> batchTests = [.. tests];

                while (batchTests.Count > 0)
                {
                    int rnd = random.Next(batchTests.Count);
                    Test<Result, Args> chosen = batchTests[rnd];
                    batchTests.RemoveAt(rnd);

                    // Only do this once and just assume that the test method is pure
                    // in order to avoid result verification or caching messing with time measurement.
                    if (chosen.Supported() && verifyResult != null && !verifyResult(args, chosen.Invoke(args)))
                    {
                        Console.Error.WriteLine();
                        Console.Error.WriteLine($"Test '{chosen.Name()}' yielded wrong result!");
                        Console.Error.WriteLine(args.ToString());
                        return;
                    }

                    chosen.Run(batchSize, args);
                }
            }
            Print(numBatches, numBatches, batchFmt);

            Console.WriteLine();
            foreach (Test<Result, Args> test in tests)
                test.PrintResult(padding);
        }

        static void Print(int i, int numBatches, string batchFmt)
            => Console.Write($"\rRunning: {i.ToString(batchFmt)}/{numBatches} = {(100.0 * i / numBatches).ToString("000.0")}%");
    }
}
