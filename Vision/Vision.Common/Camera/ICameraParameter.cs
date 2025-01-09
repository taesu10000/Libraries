namespace Common.Vision.Camera
{
    public interface ICameraParameter
    {
        double Brightness { get; set; }
        double Contrast { get; set; }
        double Exposure { get; set; }
        int LiveTriggerInterval { get; set; }
    }
}