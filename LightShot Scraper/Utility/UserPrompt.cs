using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

using LightShotScraper.Core;

namespace LightShotScraper.Utility
{
    static class UserPrompt
    {
        private static string SetUpPrefix()
        {
            Console.WriteLine("Please select a prefix:");
            string prefix;
            prefix = Console.ReadLine();

            while (true)
            {
                if (prefix.Length < 1 || prefix.Length > 6 || prefix.First() == '0')
                {
                    Console.WriteLine("Prefix must be between 1 and 6 characters! And mustn't start with a 0! Enter '-' to cancel.");
                    prefix = Console.ReadLine();

                    if (prefix == "-")
                    {
                        return prefix;
                    }

                    continue;
                }
                break;
            }
            return prefix;
        }

        private static int GetAmountToDownload()
        {
            Console.WriteLine("How many photos do you want to download?");
            int amount;

            while (true)
            {
                int.TryParse(Console.ReadLine(), out amount);

                if (amount <= 0)
                {
                    Console.WriteLine("Invalid argument! Please try again:");
                    continue;
                }
                break;
            }

            return amount;
        }

        public static void PrintOptions(out int amount, out string prefix)
        {
            Console.WriteLine("Select an option:");

            while (true)
            {
                Console.WriteLine("\n1. Generate random images.");
                Console.WriteLine("2. Generate random images by a prefix.");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        amount = GetAmountToDownload();
                        prefix = string.Empty;
                        break;
                    case "2":
                        prefix = SetUpPrefix();

                        if (prefix == "-")
                            continue;

                        amount = GetAmountToDownload();
                        break;
                    default:
                        amount = 0;
                        prefix = string.Empty;
                        Console.WriteLine("Invalid option. Please try again:");
                        break;
                }

                if (amount == 0)
                    continue;

                break;
            }
            Console.WriteLine();
        }

        public static string GetDirectory()
        {
            Console.WriteLine("Please select a directory!");
            string directory;
            while (true)
            {
                directory = Console.ReadLine();
                if (!Directory.Exists(directory))
                {
                    Console.WriteLine("No such directory exists! Please try again:");
                    continue;
                }
                break;
            }

            Console.WriteLine($"Directory set to: {directory}.\n");
            return directory;
        }

        public static List<Uri> GenerateLinks(int amount, string prefix = "")
        {
            List<Uri> uris = new List<Uri>();
            string baseUrl = "https://prnt.sc/";

            for (int i = 0; i < amount; i++)
            {
                if(prefix.Length == 6)
                {
                    amount = 0;
                }

                var url = baseUrl + GenerateImagePath.GenerateRandomPath(prefix);
                Uri.TryCreate(url, UriKind.Absolute, out var uri);
                uris.Add(uri);
            }

            return uris;
        }

        public static void DownloadImages(string directory, List<Uri> uris)
        {
            ProgressBar module = new ProgressBar(20);
            var downloader = new ImageDownloader(directory);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            downloader.DownloadImagesAsync(uris, module);

            stopwatch.Stop();

            Console.WriteLine($"\nDone! Elapsed time: {stopwatch.ElapsedMilliseconds}\n");
        }
    }
}
