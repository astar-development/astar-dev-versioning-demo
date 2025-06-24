using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AStar.Dev.Versioning.Demo;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseKestrel(options => options.AddServerHeader = false); // removes "powered-by" etc.
            });
    }
}