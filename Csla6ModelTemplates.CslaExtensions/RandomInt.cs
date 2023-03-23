namespace Csla6ModelTemplates.CslaExtensions
{
    public static class RandomInt
    {
        private static readonly Random Generator = new Random(DateTime.Now.Millisecond);

        public static int Next (
            int minValue,
            int maxValue
            )
        {
            return Generator.Next(minValue, maxValue);
        }
    }
}
