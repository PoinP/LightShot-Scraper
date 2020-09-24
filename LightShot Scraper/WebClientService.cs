using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LightShotScraper
{
    abstract class WebClientService
    {
        protected WebClient _webClient;
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:81.0) Gecko/20100101 Firefox/81.0";

        protected WebClientService()
        {
            _webClient = new WebClient();
            _webClient.Headers.Add("authority", "prnt.sc");
            _webClient.Headers.Add("cache-control", "max-age=0");
            _webClient.Headers.Add("upgrade-insecure-requests", "1");
            _webClient.Headers.Add("user-agent", UserAgent);
            //_webClient.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
        }
    }
}
