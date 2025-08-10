using System;
using System.Diagnostics;

namespace ChaosBenchmark
{
    public class Test
    {
        public delegate void TestMethod();

        public readonly string name;

        readonly Stopwatch tm = new Stopwatch();
        readonly TestMethod action;

        public Test(string name, TestMethod action)
        {
            this.name = name;
            this.action = action;
        }

        public void Run(int batchSize)
        {
            tm.Start();
            for (int i = 0; i < batchSize; i++)
                action();
            tm.Stop();
        }

        public void PrintResult(int namePadding)
            => Console.WriteLine($"{$"{name}:".PadRight(namePadding + 1)} {tm.Elapsed.TotalSeconds}");
    }
}
