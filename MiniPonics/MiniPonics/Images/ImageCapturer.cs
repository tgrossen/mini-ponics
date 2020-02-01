using System.IO;
using System.Threading.Tasks;
using MMALSharp;
using MMALSharp.Handlers;
using MMALSharp.Native;

namespace MiniPonics.Images
{
    public interface IImageCapturer
    {
        Task<byte[]> Capture();
    }

    public class ImageCapturer : IImageCapturer
    {
        public async Task<byte[]> Capture()
        {
            var cam = MMALCamera.Instance;
            using var imgCaptureHandler = new MemoryStreamCaptureHandler();
            await cam.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
            
            return imgCaptureHandler.CurrentStream.ToArray();
        }
    }
}