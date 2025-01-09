namespace Common.Vision.Cognex
{
    public interface IInspectionResult
    {
        string DecodedString { get; set; }
        byte IdentifierCode { get; set; }
        byte IdentifierModifier { get; set; }
        CogIDSymbologyConstants Symbology { get; set; }
    }
}