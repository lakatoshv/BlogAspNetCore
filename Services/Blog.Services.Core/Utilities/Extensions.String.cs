namespace Blog.Services.Core.Utilities
{
    /// <summary>
    /// String extensions.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Convert string to int.
        /// </summary>
        /// <param name="input">input.</param>
        /// <returns>int.</returns>
        public static int ToInt(this string input)
        {
            // int result = 0;
            int.TryParse(input, out var result);
            return result;
        }

        /// <summary>
        /// Convert string to bool.
        /// </summary>
        /// <param name="input">input.</param>
        /// <returns>bool.</returns>
        public static bool ToBool(this string input)
        {
            // int result = 0;
            bool.TryParse(input, out var result);
            return result;
        }
    }
}
