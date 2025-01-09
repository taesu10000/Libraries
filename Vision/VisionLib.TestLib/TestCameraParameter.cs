using Domino.Gen2.Common.Vision.Camera;

namespace VisionLib.TestLib
{
    internal class TestCameraParameter : ICameraParameter
    {
        public TestCameraParameter()
        {
            Brightness = 1;
            Contrast = 1;
            Exposure = 10;
            LiveTriggerInterval = 5;
        }
        public double Brightness { get; set; }
        public double Contrast { get; set; }
        public double Exposure { get; set; }
        public int LiveTriggerInterval { get; set; }

    }
}