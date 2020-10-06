using System;
using System.Text;

namespace LightShotScraper.Utility
{
    /// <summary>
    /// Shows a progress of the downloaded images.
    /// </summary>
	public class ProgressBar
    {
        /// <summary>
        /// Progress bar's percentage.
        /// </summary>
        private int _percentage;
        /// <summary>
        /// Loadedblocks of the progress bar.
        /// </summary>
        private int _loadedBlocks;
        /// <summary>
        /// The loading blocks' style.
        /// </summary>
        private char _block = '█';
        /// <summary>
        /// Amount of all the blocks to load.
        /// </summary>
        private int _blockCount;
        /// <summary>
        /// Properties of an image.
        /// </summary>
        private LightshotImage _image;
        /// <summary>
        /// Current progress string.
        /// </summary>
        private string _progress;
        
        /// <summary>
        /// Ctor that sets the progress bar's settings.
        /// </summary>
        /// <param name="blockCount"></param>
        public ProgressBar(int blockCount = 20)
        {
            _percentage = 0;
            _loadedBlocks = 0;
            _blockCount = blockCount;
        }

        /// <summary>
        /// Retports a successfull pass and updates the loading bar.
        /// </summary>
        /// <param name="percentage">Percentage to set the progress bar to.</param>
        /// <param name="image">Properties of an image.</param>
        public void Report(double percentage, LightshotImage image)
        {
            _image = image;
            _percentage = (int)percentage;
            double leftOver = (double)_blockCount / 10;
            double blockInterval = Math.Round(10 / leftOver, 2);

            if (_percentage >= blockInterval)
            {
                _loadedBlocks = (int)((_percentage - (_percentage % blockInterval)) / blockInterval);
            }

            UpdateText();
        }

        /// <summary>
        /// Draws the current progress bar progress on the screen.
        /// </summary>
        public void DrawCurrentProgressBar()
        {
            Console.Write($"\r{_progress}");
        }

        /// <summary>
        /// Updates the progress bar and draws it.
        /// </summary>
        private void UpdateText()
        {
            var loadingBar = GetProgressBar();

            _progress = $"Downloading: [{loadingBar}] {_percentage}%";
            Console.WriteLine($"\rDownloaded: {_image.Name}.{_image.Format.ToString().ToLower()} \\ {_image.Size}                                     ");
            Console.Write($"\r{_progress}");
        }

        /// <summary>
        /// Geneartes the progress bar based on the percentage.
        /// </summary>
        /// <returns>The progress bar.</returns>
        private string GetProgressBar()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int blocksAdded = 1;
            
            for(int i = 0; i < _blockCount; i++)
            {
                if (blocksAdded <= _loadedBlocks)
                {
                    stringBuilder.Append(_block);
                    blocksAdded++;
                    continue;
                }

                stringBuilder.Append('-');
            }

            return stringBuilder.ToString();
        }
    }
	
}
