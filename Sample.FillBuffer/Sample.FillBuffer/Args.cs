using System;
using System.Linq;

namespace Sample.FillBuffer
{
    public class Args
    {
        static readonly Random random = new();

        public static Args CreateRandom() => new();
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
            bldr.Append(string.Join(',', pattern.Select(ByteToHex)));
            bldr.AppendLine("}");
            return bldr.ToString();
        }
    }
}
