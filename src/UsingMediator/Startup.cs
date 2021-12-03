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
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureMediatorServices(IServiceCollection services)
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Using Mediator", Version = "v1" });
            });
        }

        public void ConfigureRabbitServices(IServiceCollection services)
        {
            //services.AddMassTransit(cfg =>
            //{
            //    cfg.AddConsumer<SubmitOrderConsumer>();
            //
            //    before version 7 mediator is added here: cfg.AddMediator(...)
            //    cfg.AddRequestClient<SubmitOrder>();
            //
            //});

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Using RabbitMQ", Version = "v1" });
            });
        }

        public void ConfigureMediator(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Now configure mediator specifics", env.EnvironmentName);
            ConfigureDeveloperFriendlyFeatures(app);
            Configure(app, env, logger);
        }

        public void ConfigureRabbit(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Now configure rabbit specifics", env.EnvironmentName);
            ConfigureDeveloperFriendlyFeatures(app);
            Configure(app, env, logger);
        }

        private static void ConfigureDeveloperFriendlyFeatures(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsingMediator v1"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Current Environment is {environmentName}", env.EnvironmentName);

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