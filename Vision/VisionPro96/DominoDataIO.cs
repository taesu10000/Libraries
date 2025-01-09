using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Vision.Camera;
using Common.Vision.File;

namespace VisionPro96V2
{
    public class DominoDataIO : IDominoDataIO
    {
        public string SaveOriginalImageFolderPath { get; set; }
        public string SaveOverlayImageFolderPath { get; set; }
        public SaveImageFormatType SaveImageFormat { get; set; }

        public event EventHandler<string> OnOriginalImageFromFileSaved;
        public event EventHandler<string> OnOverlayImageFromFileSaved;

        public void SaveOriginalImage(IDominoImage overlayImage)
        {
            
        }

        public void SaveOverlayImage(IDominoImage overlayImage)
        {
            throw new NotImplementedException();
        }
    }
}
