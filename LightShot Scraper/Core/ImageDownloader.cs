using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

using LightShotScraper.Utility;

namespace LightShotScraper.Core
{
    /// <summary>
    /// Downlaods the images
    /// </summary>
    class ImageDownloader : WebClientService
    {
        /// <summary>
        /// The image's path.
        /// </summary>
        private string _path;
        /// <summary>
        /// The image's properties.
        /// </summary>
        private LightshotImage _image;

        /// <summary>
        /// Ctor. Set's the path for the image to be saved to.
        /// </summary>
        /// <param name="path">User desierd path to save images to.</param>
        public ImageDownloader(string path = "") : base()
        {
            _path = path;
        }

        /// <summary>
        /// Downloads an image.
        /// </summary>
        /// <param name="uri">An image's uri.</param>
        /// <returns>The image's properties.</returns>
        public LightshotImage DownloadImage(Uri uri)
        {
            _image = new LightshotImage(uri);

            if (IsImageValid())
            {
                return SaveImage();
            }
            return null;
        }

        /// <summary>
        /// Downloads a list of images asynchronously.
        /// </summary>
        /// <param name="uris">Uri list of the images.</param>
        /// <param name="progress">Progress object tied to the progress bar.</param>
        /// <returns>A list of properties of the images downloaded.</returns>
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

        /// <summary>
        /// Get a formatted file path for an image.
        /// </summary>
        /// <returns>Formatted directory to save the image to.</returns>
        private string GetFilePath()
        {
            return $@"{_path}\{_image.Name}.{_image.Format}";
        }

        /// <summary>
        /// Saves an image.
        /// </summary>
        /// <returns>An image property object.</returns>
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

        /// <summary>
        /// Checks if an image exists in the site or not.
        /// </summary>
        /// <returns>If an image exists in the site.</returns>
        private bool IsImageValid()
        {
            return !_image.Uri.AbsoluteUri.Contains("st.prntscr.com/2020/08/01/0537/img/0_173a7b_211be8ff.png");
        }

        /// <summary>
        /// Checks if an image with the same name exists in the desired directory.
        /// </summary>
        /// <returns>If an image exists or not.</returns>
        private bool DoesImageExist()
        {
            return File.Exists(GetFilePath());
        }
    }
}
