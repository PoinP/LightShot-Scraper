using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LightShotScraper.Core
{
    /// <summary>
    /// Parses the html data of a Lightshot url.
    /// </summary>
    class HtmlParser : WebClientService
    {
        /// <summary>
        /// Checks if the web agent has been changed.
        /// </summary>
        bool isUserAgentChanged;

        /// <summary>
        /// Ctor
        /// </summary>
        public HtmlParser() : base()
        {
            isUserAgentChanged = false;
        }

        /// <summary>
        /// Gets the image uri from a Ligtshot uri.
        /// </summary>
        /// <param name="uri">Lightshot uri.</param>
        /// <returns>The image uri.</returns>
        public Uri GetImageUrl(Uri uri)
        {
            var htmlData = _webClient.DownloadString(uri);

            if (!isUserAgentChanged)
            {
                _webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36");
                isUserAgentChanged = true;
            }

            return ParseImageUrl(htmlData);
        }

        /// <summary>
        /// Gets the image uri from a Ligtshot url.
        /// </summary>
        /// <param name="uri">Lightshot uri.</param>
        /// <returns>The image uri.</returns>
        public Uri GetImageUrl(string url)
        {
            if(!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new UriFormatException("Url was not formatted correctly!");
            }

            Uri.TryCreate(url, UriKind.Absolute, out var uri);

            return GetImageUrl(uri);
        }

        /// <summary>
        /// Gets the image uri from a Ligtshot uri asynchronously.
        /// </summary>
        /// <param name="uri">Lightshot uri.</param>
        /// <returns>The image uri.</returns>
        public async Task<Uri> GetImageUrlAsync(Uri uri)
        {
            var htmlData = await _webClient.DownloadStringTaskAsync(uri);

            return ParseImageUrl(htmlData);
        }

        /// <summary>
        /// Gets the uri of the images from a Ligtshot uri list asynchronously.
        /// </summary>
        /// <param name="uri">Lightshot uri.</param>
        /// <returns>The image uri.</returns>
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

        /// <summary>
        /// Parses the HTML data of a Lightshot url.
        /// </summary>
        /// <param name="url">Ligthshot url.</param>
        /// <returns>The image's url.</returns>
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
