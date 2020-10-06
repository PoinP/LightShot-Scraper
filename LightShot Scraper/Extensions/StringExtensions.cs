using System.Linq;

namespace System
{
    /// <summary>
    /// Helpful string extensions
    /// </summary>
    static class StringExtensions
    {
        /// <summary>
        /// Gets the string between two substrings.
        /// </summary>
        /// <param name="str">The whole string.</param>
        /// <param name="firstSubstring">The first substring.</param>
        /// <param name="secondSubstring">The second substring.</param>
        /// <returns>The string between the first and second substring</returns>
        public static string GetStringBetween(this string str, string firstSubstring, string secondSubstring)
        {
            int p1 = str.IndexOf(firstSubstring) + firstSubstring.Length;
            string leftOver = str.Remove(0, p1);
            int p2 = p1 + leftOver.IndexOf(secondSubstring);

            return str.Substring(p1, p2 - p1);
        }

        /// <summary>
        /// Chnages the first element of a string to it's uppercase variant.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The string with it's first element as an uppercase variant.</returns>
        public static string FirstCharToUpper(this string str)
        {
            if(String.IsNullOrEmpty(str))
            {
                throw new ArgumentException($"{nameof(str)} can not be null or empty!");
            }

            return str.First().ToString().ToUpper() + str.Substring(1);
        }
    }
}
