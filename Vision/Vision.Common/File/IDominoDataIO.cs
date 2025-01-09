using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Vision.Camera;

namespace Common.Vision.File
{
    public interface IDominoDataIO
    {
        string SaveOriginalImageFolderPath { get; set; }
        string SaveOverlayImageFolderPath { get; set; }
        SaveImageFormatType SaveImageFormat { get; set; }
        event EventHandler<string> OnOriginalImageFromFileSaved;
        event EventHandler<string> OnOverlayImageFromFileSaved;

        void SaveOriginalImage(IDominoImage overlayImage);
        void SaveOverlayImage(IDominoImage overlayImage);
    }
}
