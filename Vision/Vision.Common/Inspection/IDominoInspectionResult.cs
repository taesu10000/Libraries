using Common.Vision.Camera;

namespace Common.Vision.Insepction
{
    public interface IDominoInspectionResult
    {
        IDominoImage OverlayImage { get; }
        IDominoImage OriginalImage { get; }
    }
}