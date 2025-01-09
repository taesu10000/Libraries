using System;
using System.Collections.Generic;
using System.Drawing;

using Cognex.VisionPro;
using Cognex.VisionPro.ID;

namespace Common.Vision.Cognex
{
    public interface IVisionResult : IDisposable
    {
        bool BarcodeResult { get; set; }
        List<IInspectionResult> CogIDResults { get; set; }
        int Count { get; }
        bool GrabResult { get; set; }
        Image Image { get; set; }
        DateTime InspectionTime { get; set; }
        List<CogGraphicCollection> GraphicCollections { get; set; }

        void Convert(CogIDResults cogIDResults);
    }
}