namespace Sample.FillBuffer.Tests
{
    using static Program;

    static class NaiveCopy
    {
        public static byte[] Perform()
        {
            byte[] result = new byte[HEIGHT * stride];

            for (int y = 0; y < HEIGHT; y++)
                for (int x = 0; x < WIDTH; x++)
                    for (int i = 0; i < pattern.Length; i++)
                        result[y * stride + x * pattern.Length + i] = pattern[i];

            return result;
        }
    }
}
