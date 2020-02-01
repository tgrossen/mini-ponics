using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniPonics.Monitor.DependencyResolution;
using MiniPonics.Monitor.Images;

namespace MiniPonics.Monitor
{
    static class Program
    {
        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            CreateHostBuilder(args).Build().Run();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    MonitorIocConfigurator.Configure(hostContext, services);
                    StartMonitorServices(services);
                });

        static void StartMonitorServices(IServiceCollection services)
        {
            services.AddHostedService<ImageMonitor>();
        }
    }
}