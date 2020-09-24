using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LightShotScraper
{
    class HtmlParser : WebClientService
    {
        bool isUserAgentChanged;

        public HtmlParser() : base()
        {
            isUserAgentChanged = false;
        }

        public Uri GetImageUrl(Uri uri)
        {
            var htmlData = _webClient.DownloadString(uri);

            if (!isUserAgentChanged)
            {
                _webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36");
            }

            return ParseImageUrl(htmlData);
        }

        public Uri GetImageUrl(string url)
        {
            if(!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new UriFormatException("Url was not formatted correctly!");
            }

            Uri.TryCreate(url, UriKind.Absolute, out var uri);

            return GetImageUrl(uri);
        }

        public async Task<Uri> GetImageUrlAsync(Uri uri)
        {
            var htmlData = await _webClient.DownloadStringTaskAsync(uri);

            return ParseImageUrl(htmlData);
        }

        public async Task<List<Uri>> GetImagesUrlsAsync(List<Uri> uris)
        {
            List<Task<Uri>> tasks = new List<Task<Uri>>();

            foreach(var uri in uris)
            {
                tasks.Add(GetImageUrlAsync(uri));
            }

            var parsedUris = await Task.WhenAll(tasks);

            return parsedUris.ToList();
        }

        private Uri ParseImageUrl(string url)
        {
            var imageUrl = url
                .GetStringBetween("image_url=", @">")
                .Replace('"', ' ')
                .Trim();

            if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var imageUri))
            {
                throw new UriFormatException("ImageUrl was not formatted correctly!");
            }

            return imageUri;
        }
    }
}
