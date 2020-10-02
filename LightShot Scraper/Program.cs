using System;
using System.Net;
using System.Xml;
using System.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace LightShotScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory;
            Console.WriteLine("Select a directory!");
            directory = Console.ReadLine();

            Console.WriteLine("How many photos do you want to download?");
            int amount = int.Parse(Console.ReadLine());

            ProgressModule module = new ProgressModule(20);

            var downloader = new ImageDownloader(directory);
            Random random = new Random();

            List<Uri> uris = new List<Uri>();
            List<string> urls = new List<string>();
            string url = "https://prnt.sc/sn";

            for (int i = 0; i < amount; i++)
            {
                var number = random.Next(1000, 9999);
                urls.Add(url + number.ToString());
            }

            foreach (var urla in urls)
            {
                Uri.TryCreate(urla, UriKind.Absolute, out var uria);
                uris.Add(uria);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            downloader.DownloadImagesAsync(uris, module);

            stopwatch.Stop();

            Console.WriteLine($"\nDone! Elapsed time: {stopwatch.ElapsedMilliseconds}");

        }

        void Test()
        {
            ProgressModule module = new ProgressModule(10);

            //for (int i = 1; i <= 100; i++)
            //{
            //    module.Report((i * 100) / 100);
            //    Thread.Sleep(200);
            //}

            //Console.WriteLine("LAMOO");
            //Thread.Sleep(999000);

            Random random = new Random();
            string url = "https://prnt.sc/sn";

            List<string> urls = new List<string>();

            for (int i = 0; i < 200; i++)
            {
                var number = random.Next(1000, 9999);
                urls.Add(url + number.ToString());
            }

            List<Uri> uris = new List<Uri>();

            // string url = "https://prnt.sc/ip7695";
            //string url = "https://prnt.sc/ip7895";

            Uri.TryCreate(url, UriKind.Absolute, out var uri);
            Uri.TryCreate("https://prnt.sc/ip9765", UriKind.Absolute, out var uri2);

            foreach (var urla in urls)
            {
                Uri.TryCreate(urla, UriKind.Absolute, out var uria);
                uris.Add(uria);
            }

            var downloader = new ImageDownloader(@"D:\Programming\C#\Test2");

            var stopwatch = new Stopwatch();

            Console.WriteLine("Downloading image...");
            stopwatch.Start();

            //downloader.DownloadImage(uri);
            downloader.DownloadImagesAsync(uris, module);
            stopwatch.Stop();

            Console.Write("");
            Console.WriteLine($"Done! Elapsed time: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
