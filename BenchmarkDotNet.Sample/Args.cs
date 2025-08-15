using System;
using System.Linq;

namespace BenchmarkDotNetSample
{
    public class Args
    {
        // TODO: figure out how to properly randomize parameters with BenchmarkDotNet
        readonly Random random = new Random(1337);

        public static Args CreateRandom() => new Args();
        static string ByteToHex(byte b) => b.ToString("X2");

        public readonly int width;
        public readonly int height;
        public readonly int stride;
        public readonly byte[] pattern;

        /// <summary> Creates random args. </summary>
        public Args()
        {
            int patternLength = random.Next(2, 64);

            width = random.Next(128, 2048 + 1);
            height = random.Next(128, 2048 + 1);

            pattern = new byte[patternLength];
            random.NextBytes(pattern);

            // up to one byte less than another full row
            stride = (width + random.Next(0, width)) * patternLength;
        }

        public override string ToString()
        {
            System.Text.StringBuilder bldr = new System.Text.StringBuilder();
            bldr.Append(nameof(width));
            bldr.Append(":   ");
            bldr.AppendLine(width.ToString());
            bldr.Append(nameof(height));
            bldr.Append(":  ");
            bldr.AppendLine(height.ToString());
            bldr.Append(nameof(stride));
            bldr.Append(":  ");
            bldr.AppendLine(stride.ToString());
            bldr.Append(nameof(pattern));
            bldr.Append(": {");
            bldr.Append(string.Join(", ", pattern.Select(ByteToHex)));
            bldr.AppendLine("}");
            return bldr.ToString();
        }
    }
}
