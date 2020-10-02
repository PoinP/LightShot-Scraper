using LightShot_Scraper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LightShotScraper
{
    class ImageDownloader : WebClientService
    {
        private string _path;
        private LightshotImage _image;

        public ImageDownloader(string path = "") : base()
        {
            _path = path;
        }

        public LightshotImage DownloadImage(Uri uri)
        {
            _image = new LightshotImage(uri);
            return SaveImage();
        }

        public List<LightshotImage> DownloadImagesAsync(List<Uri> uris, ProgressModule progress)
        {
            var images = new List<LightshotImage>();
            int count = 0;
            foreach (var uri in uris)
            {
                count++;
                _image = new LightshotImage(uri);
                images.Add(DownloadImage(uri));
                progress.Report((count * 100) / uris.Count, _image);
            }

            return images;
        }
        private string GetFilePath()
        {
            return $@"{_path}\{_image.Name}.{_image.Format}";
        }

        private LightshotImage SaveImage()
        {
            var imagePath = GetFilePath();

            if(_image.Uri.AbsoluteUri.Contains("st.prntscr.com/2020/08/01/0537/img/0_173a7b_211be8ff.png"))
            {
                return null;
            }

            using var stream = _webClient.OpenRead(_image.Uri.AbsoluteUri);
            var imageSize = Convert.ToInt64(_webClient.ResponseHeaders["Content-Length"]);
            _image.SetFileSize(imageSize);
            Bitmap bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(imagePath, _image.Format);
            }

            return _image;
        }
    }
}
