namespace Direct4Me.Core.ImageProccessing.Interfaces;

public interface IImageProcessingService
{
    Task<object> CompressImageAsync(object imageFile);
}