using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniPonics.Settings;
using NCrontab;

namespace MiniPonics.Monitor.DependencyResolution
{
    public static class MonitorIocConfigurator
    {
        public static void Configure(HostBuilderContext context, IServiceCollection services)
        {
            var settings = new MiniPonicsSettings();

            services.Scan(scanner =>
            {
                scanner.FromAssemblyOf<MiniPonicsAssemblyLocator>().AddClasses().AsSelfWithInterfaces().AsMatchingInterface().WithSingletonLifetime();
                scanner.FromCallingAssembly().AddClasses().AsSelf().AsMatchingInterface().WithTransientLifetime();
            });

            services.AddSingleton<IMiniPonicsSettings>(settings);
            services.AddSingleton<CrontabSchedule, CrontabSchedule>();
        }
    }
}