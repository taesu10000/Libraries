using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;

using Domino.Gen2.Common.Vision.Camera;
using Domino.Gen2.Common.Vision.Cognex;
using Domino.Gen2.Common.Vision.Insepction;

namespace Domino.Gen2.VisionPro96V2.Models
{
    public class DominoInspectionResult : ICognexInspectionResult
    {
        public IDominoImage OriginalImage { get; internal set; }
        public IDominoImage OverlayImage { get; }
        public ICogRecord Record { get; internal set; }
    }
}
