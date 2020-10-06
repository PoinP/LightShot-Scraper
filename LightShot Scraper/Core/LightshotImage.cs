using System;
using System.Drawing.Imaging;

using LightShotScraper.Core;

namespace LightShotScraper
{
    /// <summary>
    /// Image properties.
    /// </summary>
    public class LightshotImage
    {
        /// <summary>
        /// Image url.
        /// </summary>
        public string Url { get; private set; }
        /// <summary>
        /// Image uri.
        /// </summary>
        public Uri Uri { get; private set; }
        /// <summary>
        /// Image's name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Image's size.
        /// </summary>
        public string Size { get; private set; }
        /// <summary>
        /// Image's format.
        /// </summary>
        public ImageFormat Format { get; private set; }


        /// <summary>
        /// Ctor that sets the image's properties.
        /// </summary>
        /// <param name="url">An image's url.</param>
        public LightshotImage(string url)
        {
            HtmlParser parser = new HtmlParser();
            Uri.TryCreate(url, UriKind.Absolute, out var uri);
            Uri = parser.GetImageUrl(uri);
            Url = Uri.AbsoluteUri;
            Name = url.Split("sc/")[1];
            Format = GetImageFormatFromUrl();
        }

        /// <summary>
        /// Ctor that sets the image's properties.
        /// </summary>
        /// <param name="uri">An image's uri.</param>
        public LightshotImage(Uri uri)
        {
            HtmlParser parser = new HtmlParser();
            Uri = parser.GetImageUrl(uri.AbsoluteUri);
            Url = Uri.AbsoluteUri;
            Name = uri.AbsoluteUri.Split("sc/")[1];
            Format = GetImageFormatFromUrl();
        }

        /// <summary>
        /// Sets the size of an image.
        /// </summary>
        /// <param name="bytes">Size in bytes.</param>
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

        /// <summary>
        /// Gets an image's foprmat from it's url.
        /// </summary>
        /// <returns>The image's format.</returns>
        private ImageFormat GetImageFormatFromUrl()
        {
            int dotFormatIndex = Url.LastIndexOf('.');

            string imageFormatString = Url.Substring(dotFormatIndex + 1).FirstCharToUpper();

            return SeekFormat(imageFormatString);
        }

        /// <summary>
        /// Gets the image format as an ImageFormat object.
        /// </summary>
        /// <param name="formatString">Image's format in string variant.</param>
        /// <returns>An ImageFormat object with the image's format.</returns>
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
