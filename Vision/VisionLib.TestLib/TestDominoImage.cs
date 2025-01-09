using System.Drawing;

using Cognex.VisionPro;

using Domino.Gen2.Common.Vision.Cognex;
using Domino.Gen2.Common.Vision.Camera;

namespace VisionLib.TestLib
{
    internal class TestDominoImage : IDominoImage
    {
        internal Image DominoImage { get; set; }
    }
}