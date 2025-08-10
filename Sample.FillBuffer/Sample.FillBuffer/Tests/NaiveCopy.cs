namespace Sample.FillBuffer.Tests
{
    static class NaiveCopy
    {
        public static byte[] Perform(Args args)
        {
            byte[] result = new byte[args.height * args.stride];

            for (int y = 0; y < args.height; y++)
                for (int x = 0; x < args.width; x++)
                    for (int i = 0; i < args.pattern.Length; i++)
                        result[y * args.stride + x * args.pattern.Length + i] = args.pattern[i];

            return result;
        }
    }
}
