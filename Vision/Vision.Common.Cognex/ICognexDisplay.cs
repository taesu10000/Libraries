using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Vision.Cognex;
using Common.Vision.Camera;
using Cognex.VisionPro;
using Common.Vision.Insepction;

namespace Common.Vision.Cognex
{
    public interface ICognexDisplay : IDominoDisplay
    {
        
        Image GetImage();
        void UpdateRecord(IDominoInspectionResult e);
    }
}
