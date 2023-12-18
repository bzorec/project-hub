namespace Direct4Me.Core.ImageProccessing.Interfaces;

public interface IPythonAiService
{
    Task<object?> ProcessImageAsync(object compressedImage);
}