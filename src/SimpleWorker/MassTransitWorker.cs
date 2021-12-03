using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWorker
{
    internal class MassTransitWorker : BackgroundService
    {
        readonly IBus _bus;

        public MassTransitWorker(IBus bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _bus.Publish(new Message { Text = $"Bus published this message: The time is {DateTimeOffset.Now}" });

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
