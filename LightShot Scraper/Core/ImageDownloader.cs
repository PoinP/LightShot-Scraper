using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

using LightShotScraper.Utility;

namespace LightShotScraper.Core
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

            if (IsImageValid())
            {
                return SaveImage();
            }
            return null;
        }

        public List<LightshotImage> DownloadImagesAsync(List<Uri> uris, ProgressBar progress)
        {
            var images = new List<LightshotImage>();
            int count = 0;

            foreach (var uri in uris)
            {
                count++;
                _image = new LightshotImage(uri);

                if (IsImageValid() && !DoesImageExist())
                {
                    images.Add(DownloadImage(uri));
                    progress.Report((count * 100) / uris.Count, _image);
                    continue;
                }

                string message = !IsImageValid() ?
                    $"\rImage {_image.Name} does not exist on the site!                               " :
                    $"\rImage {_image.Name} already exists in the selected directory!                         ";

                Console.WriteLine(message);
                progress.DrawCurrentProgressBar();
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

        private bool IsImageValid()
        {
            return !_image.Uri.AbsoluteUri.Contains("st.prntscr.com/2020/08/01/0537/img/0_173a7b_211be8ff.png");
        }

        private bool DoesImageExist()
        {
            return File.Exists(GetFilePath());
        }
    }
}
