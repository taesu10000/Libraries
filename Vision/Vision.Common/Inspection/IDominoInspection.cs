using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Vision.Camera;

namespace Common.Vision.Insepction
{
    public interface IDominoInspection
    {
        int MaxBarcodeReadingCount { get; set; }

        void Run(IDominoImage dominoImage);
        event EventHandler<IDominoInspectionResult> OnInspectionComplete;
    }
}
