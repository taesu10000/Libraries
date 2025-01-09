using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Domino.Gen2.Common.Vision.Cognex;
using Domino.Gen2.Common.Vision.Camera;
using Domino.Gen2.Common.Vision.Insepction;

namespace VisionLib.TestLib
{
    public partial class TestDisplay : UserControl, ICognexDisplay
    {
        public TestDisplay()
        {
            InitializeComponent();
        }

        public void ClearGraphics()
        {
            Console.WriteLine("ClearGraphics");
        }

        public Image GetImage()
        {
            return new Bitmap(100, 100);
        }

        public void LoadImageFromFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void SaveImage(string fileName)
        {
            Console.WriteLine("SaveImage");
        }

        public void UpdateDisplay(Image image)
        {
            pictureBox1.Image = image;
        }

        public void UpdateDisplay(IDominoImage image)
        {
            if (image is TestDominoImage dominoImage)
            {
                pictureBox1.Image = dominoImage.DominoImage;
            }
        }

        public void UpdateRecord(IDominoInspectionResult e)
        {
            throw new NotImplementedException();
        }
    }
}
