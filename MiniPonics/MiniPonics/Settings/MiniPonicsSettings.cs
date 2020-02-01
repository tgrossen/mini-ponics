using System.IO;
using Microsoft.Extensions.Configuration;

namespace MiniPonics.Settings
{
    public interface IMiniPonicsSettings
    {
    }

    public class MiniPonicsSettings : IMiniPonicsSettings
    {
        readonly IConfigurationRoot config;

        public MiniPonicsSettings(string fileName = "appsettings.json")
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(fileName, false)
                .SetBasePath(Directory.GetCurrentDirectory());

            config = builder.Build();
        }
    }
}