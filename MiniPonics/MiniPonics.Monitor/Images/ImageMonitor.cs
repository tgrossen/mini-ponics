using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MiniPonics.HomeEvents;
using NCrontab;

namespace MiniPonics.Monitor.Images
{
    public class ImageMonitor : BackgroundService
    {
        static string Schedule => "*/5 * * * *";
        readonly CrontabSchedule schedule;
        DateTime nextRun;
        readonly IHomeEventsImageSender homeEventsImageSender;

        public ImageMonitor(IHomeEventsImageSender homeEventsImageSender)
        {
            this.homeEventsImageSender = homeEventsImageSender;
            schedule = CrontabSchedule.Parse(Schedule);
            nextRun = schedule.GetNextOccurrence(DateTime.Now);
            ;
        }

        async Task RunScheduler(CancellationToken cancellationToken)
        {
            do
            {
                var now = DateTime.Now;
                if (now.Hour >= 6 && now.Hour <= 19 && now > nextRun)
                {
                    Console.WriteLine("Capturing image.");
                    try
                    {
                        await homeEventsImageSender.CaptureAndSend();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception calling image sender: ", e);
                    }

                    nextRun = schedule.GetNextOccurrence(DateTime.Now);
                }

                await Task.Delay(1000, cancellationToken);
            } while (!cancellationToken.IsCancellationRequested);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting image monitor service");

            while (!stoppingToken.IsCancellationRequested)
            {
                await RunScheduler(stoppingToken);
            }
            
            Console.WriteLine("Stopping image monitor service");
        }
    }
}