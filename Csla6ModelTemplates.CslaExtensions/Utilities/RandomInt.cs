namespace Csla6ModelTemplates.CslaExtensions.Utilities
{
    /// <summary>
    /// Provides a method to get a random integer.
    /// </summary>
    public static class RandomInt
    {
        private static readonly Random Generator = new Random(DateTime.Now.Millisecond);

        /// <summary>
        /// Gets a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random integer returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random integer returned.</param>
        /// <returns>An integer greater or equal to minValue and less than maxValue.</returns>
        public static int Next(
            int minValue,
            int maxValue
            )
        {
            return Generator.Next(minValue, maxValue);
        }
    }
}
