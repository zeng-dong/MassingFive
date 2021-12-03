using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //UsingInMemoryTransport(services);

                    UsingAzureServiceBusTransport(hostContext, services);

                    services.AddMassTransitHostedService(true);

                    services.AddHostedService<MassTransitWorker>();
                    services.AddHostedService<AnotherPublisher>();
                });

        private static void UsingAzureServiceBusTransport(HostBuilderContext hostContext, IServiceCollection services)
        {
            IConfiguration configuration = hostContext.Configuration;
            var connectionString = configuration.GetConnectionString("AzureServiceBus");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<MessageConsumer>();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(connectionString);

                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        private static void UsingInMemoryTransport(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<MessageConsumer>();

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
