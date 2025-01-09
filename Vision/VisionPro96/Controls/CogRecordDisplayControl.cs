using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Common.Vision.Cognex;
using Common.Vision.Camera;
using Cognex.VisionPro;
using Common.Vision.Insepction;

namespace VisionPro96V2.Controls
{
    public partial class CogRecordDisplayControl : UserControl, ICognexDisplay
    {
        public CogRecordDisplayControl()
        {
            InitializeComponent();
        }

        public void ClearGraphics()
        {
            throw new NotImplementedException();
        }

        public Image GetImage()
        {
            throw new NotImplementedException();
        }

        public void LoadImageFromFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void SaveImage(string fileName)
        {
            throw new NotImplementedException();
        }

        public void UpdateDisplay(Image image)
        {
            throw new NotImplementedException();
        }

        public void UpdateDisplay(IDominoImage dominoImage)
        {
            if (dominoImage is ICognexImage image)
            {
                cogRecordDisplay1.Image = image.CogImage;
            }
        }

        public void UpdateRecord(IDominoInspectionResult e)
        {
            if (e is ICognexInspectionResult result)
            {
                if (result.Record == null)
                {
                    var cognexImage = (result.OriginalImage as ICognexImage);
                    if (cognexImage != null)
                    {
                        cogRecordDisplay1.Image = cognexImage.CogImage;
                    }
                }
                else
                    cogRecordDisplay1.Record = result.Record;
            }
            else
            {
                throw new ArgumentException("Invalid inspection result type");
            }
        }
    }
}
