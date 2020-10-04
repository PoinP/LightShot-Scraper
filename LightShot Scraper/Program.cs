using LightShotScraper.Utility;

namespace LightShotScraper.Core
{
    class Program
    {
        static void Main(string[] args)
        {
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
