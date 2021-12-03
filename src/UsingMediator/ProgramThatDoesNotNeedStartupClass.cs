using Components.Consumers;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;


namespace UsingMediator
{
    public class ProgramThatDoesNotNeedStartupClass
    {
        public static void Change_To_Main_When_You_Want_To_Try_This(string[] args)
        //public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // instead of using Startup
                    webBuilder
                        .ConfigureServices(ConfigureServices)
                        .Configure(Configure);

                });

        public static void ConfigureServices(IServiceCollection services)
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UsingMediator", Version = "v1" });
            });
        }

        public static void Configure(IApplicationBuilder app)
        {
            // get this manually 
            IWebHostEnvironment env = app.ApplicationServices.GetService<IWebHostEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsingMediator v1"));
            }

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
