﻿using System;
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
            Random random = new Random();
            string url = "https://prnt.sc/ip";

            List<string> urls = new List<string>();
            
            for(int i = 0; i < 100; i++)
            {
                var number = random.Next(1000, 9999);
                urls.Add(url + number.ToString());
            }

            List<Uri> uris = new List<Uri>();

            // string url = "https://prnt.sc/ip7695";
            //string url = "https://prnt.sc/ip7895";

            Uri.TryCreate(url, UriKind.Absolute, out var uri);
            Uri.TryCreate("https://prnt.sc/ip9765", UriKind.Absolute, out var uri2);

            foreach(var urla in urls)
            {
                Uri.TryCreate(urla, UriKind.Absolute, out var uria);
                uris.Add(uria);
            }

            var downloader = new ImageDownloader(@"D:\LightShot");

            var stopwatch = new Stopwatch();

            Console.WriteLine("Downloading image...");
            stopwatch.Start();

            //downloader.DownloadImage(uri);
            downloader.DownloadImagesAsync(uris);
            stopwatch.Stop();

            Console.WriteLine($"Done! Elapsed time: {stopwatch.ElapsedMilliseconds}");
        }
    }
}