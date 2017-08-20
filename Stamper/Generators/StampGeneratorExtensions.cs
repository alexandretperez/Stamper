namespace Stamper.Generators
{
    public static class StampGeneratorExtensions
    {
        public static bool Vary<T>(this StampGenerator<T, bool> generator) where T : class
        {
            return generator.GetRandomizer().Next(2) == 1;
        }

        public static int Between<T>(this StampGenerator<T, int> generator, int min, int max) where T : class
        {
            if (max < int.MaxValue)
                max++;

            return generator.GetRandomizer().Next(min, max);
        }
    }
}