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
    public class StartupMediator
    {
        public StartupMediator(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediator(cfg =>
            {
                cfg.AddConsumer<SubmitOrderConsumer>();
                cfg.AddConsumer<GetWeatherForecastConsumer>();

                cfg.AddRequestClient<SubmitOrder>();

                // Remove this if you are only using this request client by creating it on the fly
                cfg.AddRequestClient<GetWeatherForecasts>();
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Using Mediator Startup by StartupMediator", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<StartupMediator> logger)
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

        private static void ConfigureDeveloperFriendlyFeatures(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsingMediator v1"));
        }
    }
}