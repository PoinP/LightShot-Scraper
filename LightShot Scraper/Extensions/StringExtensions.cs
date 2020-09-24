
using System.Diagnostics;
using System.Linq;

namespace System
{
    static class StringExtensions
    {
        public static string GetStringBetween(this string str, string firstIndex, string secondIndex)
        {
            int p1 = str.IndexOf(firstIndex) + firstIndex.Length;
            string leftOver = str.Remove(0, p1);
            int p2 = p1 + leftOver.IndexOf(secondIndex);

            return str.Substring(p1, p2 - p1);
        }

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
