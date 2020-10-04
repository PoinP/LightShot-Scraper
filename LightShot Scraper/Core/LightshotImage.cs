using System;
using System.Drawing.Imaging;

using LightShotScraper.Core;

namespace LightShotScraper
{
    public class LightshotImage
    {
        public string Url { get; private set; }
        public Uri Uri { get; private set; }
        public string Name { get; private set; }
        public string Size { get; private set; }
        public ImageFormat Format { get; private set; }


        public LightshotImage(string url)
        {
            HtmlParser parser = new HtmlParser();
            Uri.TryCreate(url, UriKind.Absolute, out var uri);
            Uri = parser.GetImageUrl(uri);
            Url = Uri.AbsoluteUri;
            Name = url.Split("sc/")[1];
            Format = GetImageFormatFromUrl();
        }
        
        public LightshotImage(Uri uri)
        {
            HtmlParser parser = new HtmlParser();
            Uri = parser.GetImageUrl(uri.AbsoluteUri);
            Url = Uri.AbsoluteUri;
            Name = uri.AbsoluteUri.Split("sc/")[1];
            Format = GetImageFormatFromUrl();
        }

        public void SetFileSize(long bytes)
        {
            Unit unit = Unit.B;
            double size = 0;

            if (bytes < 1024)
            {
                unit = Unit.B;
                Size = $"{bytes} {unit}";
                return;
            }
            if (bytes > 1024 && bytes < Math.Pow(1024, 2))
            {
                unit = Unit.KB;
                size = bytes / 1024;
            }
            else if (bytes > Math.Pow(1024, 2) && bytes < Math.Pow(1024, 3))
            {
                unit = Unit.MB;
                size = bytes / Math.Pow(1024, 2);
            }
            else if (bytes > Math.Pow(1024, 3) && bytes < Math.Pow(1024, 4))
            {
                unit = Unit.GB;
                size = bytes / Math.Pow(1024, 3);
            }
            else if (bytes > Math.Pow(1024, 4))
            {
                unit = Unit.TB;
                size = bytes / Math.Pow(1024, 4);
            }

            Size = $"{Math.Round(size, 2)} {unit}";
        }

        private ImageFormat GetImageFormatFromUrl()
        {
            int dotFormatIndex = Url.LastIndexOf('.');

            string imageFormatString = Url.Substring(dotFormatIndex + 1).FirstCharToUpper();

            return SeekFormat(imageFormatString);
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
                "Jpg" => ImageFormat.Jpeg,
                "Png" => ImageFormat.Png,
                "Tiff" => ImageFormat.Tiff,
                "Wmf" => ImageFormat.Wmf,
                _ => throw new ArgumentException("No such format exists!")
            };
    }
}
