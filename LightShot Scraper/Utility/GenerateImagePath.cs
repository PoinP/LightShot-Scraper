using System;
using System.Linq;

namespace LightShotScraper.Utility
{
    /// <summary>
    /// Used to generate image urls.
    /// </summary>
    static class GenerateImagePath
    {
        /// <summary>
        /// Instance of the random class used to randomize urls.
        /// </summary>
        private static readonly Random _random = new Random();
        /// <summary>
        /// Charset sued to generate urls.
        /// </summary>
        private const string _charset = "abcdefghijklmnopqrstuvwxyz1234567891234567890";

        /// <summary>
        /// Gets random character from the defined charset.
        /// </summary>
        /// <param name="str">Used for the "Select" LINQ method.</param>
        /// <returns>A single character.</returns>
        private static char GetRandomCharacter(string str)
        {
            return _charset[_random.Next(_charset.Length)];
        }

        /// <summary>
        /// Generates a random image url path. Ex: prnt.sc/ip8763 | Generates ip8763
        /// </summary>
        /// <param name="prefix">A user defined prefix. Ex. ip8 is a prefix and will generate elements after ip8.</param>
        /// <returns>The generated url path.</returns>
        public static string GenerateRandomPath(string prefix = "")
        {
            var difference = 6 - prefix.Length;

            if(difference < 0)
            {
                return null;
            }

            var characters = Enumerable.Repeat(_charset, difference).Select((GetRandomCharacter)).ToArray();

            try // This try catch block is used because of the "GetRandomCharacter" method. It throws and exception is the prefix has 6 elements. 
            {
                if (characters[0] == '0')
                {
                    char c;
                    while (true)
                    {
                        c = GetRandomCharacter("");
                        if (c != '0')
                        {
                            break;
                        }
                    }
                    characters[0] = c;
                }
            }
            catch { }

            return  prefix + string.Join("", characters);
        }
    }
}
