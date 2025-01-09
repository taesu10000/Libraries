using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cognex.VisionPro;

using Common.Vision.Camera;

namespace VisionPro96V2
{
    internal class DominoCameraParameter : ICameraParameter
    {
        private readonly ICogAcqFifo _acqFifo;

        public DominoCameraParameter(ICogAcqFifo acqFifo)
        {
            _acqFifo = acqFifo;
        }

        public double Brightness { get => _acqFifo.OwnedBrightnessParams.Brightness; set => _acqFifo.OwnedBrightnessParams.Brightness = value; }
        public double Contrast { get => _acqFifo.OwnedContrastParams.Contrast; set => _acqFifo.OwnedContrastParams.Contrast = value; }
        public double Exposure { get => _acqFifo.OwnedExposureParams.Exposure; set => _acqFifo.OwnedExposureParams.Exposure = value; }
        public int LiveTriggerInterval { get; set; } = 5;
    }
}
