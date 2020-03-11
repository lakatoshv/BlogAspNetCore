namespace Blog.Services.Core.Utilities
{
    public static partial class Extensions
    {
        public static int ToInt(this string input)
        {
            int result = 0;
            int.TryParse(input, out result);
            return result;
        }

        public static bool ToBool(this string input)
        {
            bool result = false;
            bool.TryParse(input, out result);
            return result;
        }
    }
}
