using System.Net;

namespace LightShotScraper.Core
{
    /// <summary>
    /// WebClient object to connect to the website.
    /// </summary>
    abstract class WebClientService
    {
        /// <summary>
        /// WebClient object to connect to the website.
        /// </summary>
        protected WebClient _webClient;
        /// <summary>
        /// Default user agent.
        /// </summary>
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:81.0) Gecko/20100101 Firefox/81.0";

        /// <summary>
        /// Ctor that sets the headers of the WebClient object.
        /// </summary>
        protected WebClientService()
        {
            _webClient = new WebClient();
            _webClient.Headers.Add("authority", "prnt.sc");
            _webClient.Headers.Add("cache-control", "max-age=0");
            _webClient.Headers.Add("upgrade-insecure-requests", "1");
            _webClient.Headers.Add("user-agent", UserAgent);
        }
    }
}
