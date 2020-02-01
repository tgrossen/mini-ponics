using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MiniPonics.Images;

namespace MiniPonics.HomeEvents
{
    public interface IHomeEventsImageSender
    {
        Task CaptureAndSend();
    }

    public class HomeEventsImageSender : IHomeEventsImageSender
    {
        readonly IImageCapturer imageCapturer;

        public HomeEventsImageSender(IImageCapturer imageCapturer)
        {
            this.imageCapturer = imageCapturer;
        }

        public async Task CaptureAndSend()
        {
            var imageBytes = await imageCapturer.Capture();
            
            var byteArrayContent = new ByteArrayContent(imageBytes);
            byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpg");

            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10),
                BaseAddress = new Uri("http://192.168.1.17:2121")
            };
            var result = await client.PostAsync("api/mini-ponics/image", new MultipartFormDataContent
            {
                {byteArrayContent, "\"file\"", "\"image.jpg\""}
            });
            
            result.EnsureSuccessStatusCode();
            Console.WriteLine("Image sent successfully");
        }
    }
}