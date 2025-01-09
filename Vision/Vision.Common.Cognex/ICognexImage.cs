using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;

using Common.Vision.Camera;

namespace Common.Vision.Cognex
{
    public interface ICognexImage : IDominoImage
    {
        ICogImage CogImage { get; }
    }
}
