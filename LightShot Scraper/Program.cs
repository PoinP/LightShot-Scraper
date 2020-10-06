using LightShotScraper.Utility;
using System;

namespace LightShotScraper.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "PoinP's Lightshot scraper";

            var directory = UserPrompt.GetDirectory();

            while (true)
            {
                UserPrompt.PrintOptions(out var amount, out var prefix);

                var links = UserPrompt.GenerateLinks(amount, prefix);
                UserPrompt.DownloadImages(directory, links);
            }
        }


    }
}
