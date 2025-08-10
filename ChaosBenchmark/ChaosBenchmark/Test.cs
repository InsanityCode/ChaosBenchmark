using System;
using System.Diagnostics;

namespace ChaosBenchmark
{
    public class Test<Result, Args>
    {
        public delegate Result TestMethod(Args args);

        public readonly string name;
        public readonly TestMethod test;

        readonly Stopwatch tm = new Stopwatch();

        public Test(string name, TestMethod test)
        {
            this.name = name;
            this.test = test;
        }

        public void Run(int batchSize, Args args)
        {
            tm.Start();
            for (int i = 0; i < batchSize; i++)
                test(args);
            tm.Stop();
        }

        public void PrintResult(int namePadding)
            => Console.WriteLine($"{$"{name}:".PadRight(namePadding + 1)} {tm.Elapsed.TotalSeconds}");
    }
}
