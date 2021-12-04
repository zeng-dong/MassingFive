using Components.Consumers;
using EventContracts;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleService
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.Title = "Notification";

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                //cfg.ReceiveEndpoint("event-listener", e =>
                //{
                //    e.Consumer(() => new SubmitOrderConsumer());
                //    e.Consumer<SubmitOrderConsumer>();
                //});
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }

    internal class EventConsumer :
            IConsumer<ValueEntered>
    {
        public async Task Consume(ConsumeContext<ValueEntered> context)
        {
            Console.WriteLine("Value: {0}", context.Message.Value);
        }
    }
}

namespace EventContracts
{
    public interface ValueEntered
    {
        string Value { get; }
    }
}