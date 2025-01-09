using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Common.Vision.Camera
{
    public interface IDominoDisplay
    {
        void ClearGraphics();
        void SaveImage(string fileName);
        void UpdateDisplay(Image image);
        void UpdateDisplay(IDominoImage image);
        void LoadImageFromFile(string fileName);
    }
}
