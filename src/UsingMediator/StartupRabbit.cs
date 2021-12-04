using Components.Consumers;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace UsingMediator
{
    public class StartupRabbit
    {
        public StartupRabbit(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureRabbitServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Using RabbitMQ Startup by StartupRabbit", Version = "v1" });
            });

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq();

                x.AddConsumer<SubmitOrderConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint("event-listener", e =>
                    {
                        e.ConfigureConsumer<SubmitOrderConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }

        private static void ConfigureDeveloperFriendlyFeatures(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsingRabbit v1"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<StartupRabbit> logger)
        {
            logger.LogInformation("Current Environment is {environmentName}", env.EnvironmentName);
            ConfigureDeveloperFriendlyFeatures(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}