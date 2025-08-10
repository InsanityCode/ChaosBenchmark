using System;
using System.Diagnostics;

namespace ChaosBenchmark
{
    public abstract class Test<Result, Args>
    {
        public delegate Result TestMethod(Args args);

        readonly Stopwatch tm = new();

        public abstract string Name();
        public abstract bool Supported();
        public abstract Result Invoke(Args args);

        public void Run(int batchSize, Args args)
        {
            if (Supported())
            {
                tm.Start();
                for (int i = 0; i < batchSize; i++)
                    Invoke(args);
                tm.Stop();
            }
        }

        public void PrintResult(int namePadding)
            => Console.WriteLine(
                $"{$"{Name()}:".PadRight(namePadding + 1)} {(Supported() ? tm.Elapsed.TotalSeconds.ToString() : "unsupported")}"
                );
    }
}
