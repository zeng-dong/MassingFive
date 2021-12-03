using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace UsingMediator
{
    public class Program
    {
        //public static void TemprarilyDisabledMain(string[] args)
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseStartup<Startup>();
                    webBuilder.UseStartup(typeof(Startup).Assembly.FullName);
                });
    }
}
