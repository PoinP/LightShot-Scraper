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
        private HtmlParser _htmlParser;
        private string _path;

        public ImageDownloader(string path = "") : base()
        {
            _htmlParser = new HtmlParser();
            _path = path;
        }

        public void DownloadImage(Uri uri)
        {
            var imageUri = _htmlParser.GetImageUrl(uri);
            SaveImage(uri, imageUri);
        }

        public async Task DownloadImageAsync(Uri uri)
        {
            var imageUri = await _htmlParser.GetImageUrlAsync(uri);
            SaveImage(uri, imageUri);
        }

        public void DownloadImagesAsync(List<Uri> uris)
        {
            foreach (var uri in uris)
            {
                DownloadImage(uri);
                Console.WriteLine(uri.AbsoluteUri.Split("sc/")[1]);
            }
        }

        private ImageFormat GetImageFormatFromUri(Uri uri)
        {
            string url = uri.AbsoluteUri;
            int dotFormatIndex = url.LastIndexOf('.');

            string imageFormatString = url.Substring(dotFormatIndex + 1).FirstCharToUpper();

            return SeekFormat(imageFormatString);
        }

        private string GetFilePath(Uri linkUri, Uri imageUri)
        {
            string fileName = linkUri.AbsoluteUri.Split("sc/")[1];
            string fileFormat = GetImageFormatFromUri(imageUri).ToString();

            return $@"{_path}\{fileName}.{fileFormat}";
        }

        private void SaveImage(Uri linkUri, Uri imageUri)
        {
            var imageFormat = GetImageFormatFromUri(imageUri);
            var imagePath = GetFilePath(linkUri, imageUri);

            if(imageUri.AbsoluteUri.Contains("st.prntscr.com/2020/08/01/0537/img/0_173a7b_211be8ff.png"))
            {
                return;
            }

            using var stream = _webClient.OpenRead(imageUri.AbsoluteUri);
            Bitmap bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(imagePath, imageFormat);
            }
        }

        private ImageFormat SeekFormat(string formatString) =>
            formatString switch
            {
                "Bmp" => ImageFormat.Bmp,
                "Emf" => ImageFormat.Emf,
                "Exif" => ImageFormat.Exif,
                "Gif" => ImageFormat.Gif,
                "Icon" => ImageFormat.Icon,
                "Jpeg" => ImageFormat.Jpeg,
                "Png" => ImageFormat.Png,
                "Tiff" => ImageFormat.Tiff,
                "Wmf" => ImageFormat.Wmf,
                 _   => throw new ArgumentException("No such format exists!")
            };
    }
}
