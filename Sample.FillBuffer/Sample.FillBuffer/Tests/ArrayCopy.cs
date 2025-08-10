using System;

namespace Sample.FillBuffer.Tests
{
    using static Program;

    static class ArrayCopy
    {
        public static byte[] Perform()
        {
            byte[] result = new byte[HEIGHT * stride];

            Array.Copy(pattern, result, pattern.Length);
            int numBytesWritten = pattern.Length;

            for (; numBytesWritten * 2 < result.Length; numBytesWritten <<= 1)
                Array.Copy(result, 0, result, numBytesWritten, numBytesWritten);

            int remainder = result.Length - numBytesWritten;
            Array.Copy(result, 0, result, numBytesWritten, remainder);

            return result;
        }
    }
}
