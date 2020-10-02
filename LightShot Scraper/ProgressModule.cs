using LightShot_Scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace LightShotScraper
{
	public class ProgressModule
    {
        private int _percentage;
        private int _loadedBlocks;
        private char _block = '█';
        private int _blockCount;
        private LightshotImage _image;
        

        public ProgressModule()
        {
            _percentage = 0;
            _loadedBlocks = 0;
            _blockCount = 20;
        }

        public ProgressModule(int blockCount)
        {
            _percentage = 0;
            _loadedBlocks = 0;
            _blockCount = blockCount;
        }

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

        private void UpdateText()
        {
            var loadingBar = GetLoadingBar();

            var progress = $"Downloading... [{loadingBar}] {_percentage}%";
            Console.WriteLine($"\rDownloading: {_image.Name}.{_image.Format.ToString().ToLower()} \\ {_image.Size}                                     ");
            Console.Write($"\r{progress}");
        }

        private string GetLoadingBar()
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
