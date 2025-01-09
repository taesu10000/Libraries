using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;

using Domino.Gen2.Common.Vision.Cognex;

namespace Domino.Gen2.VisionPro96V2
{
    internal class DominoImage : ICognexImage
    {
        public ICogImage CogImage { get; }
        public DominoImage(ICogImage img)
        {
            CogImage = img;
        }
    }
}
