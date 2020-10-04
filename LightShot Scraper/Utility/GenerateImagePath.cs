using System;
using System.Linq;

namespace LightShotScraper.Utility
{
    static class GenerateImagePath
    {
        private static readonly Random _random = new Random();
        private const string _charset = "abcdefghijklmnopqrstuvwxyz1234567891234567890";

        private static char GetRandomCharacter(string str)
        {
            return _charset[_random.Next(_charset.Length)];
        }

        public static string GenerateRandomPath(string prefix = "")
        {
            var difference = 6 - prefix.Length;

            if(difference < 0)
            {
                return null;
            }

            var characters = Enumerable.Repeat(_charset, difference).Select((GetRandomCharacter)).ToArray();

            try
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
