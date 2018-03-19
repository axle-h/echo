using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Echo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await WebHost.CreateDefaultBuilder(args)
                         .UseSerilog((ctx, cfg) => cfg.MinimumLevel.Information()
                                                      .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                                                      .WriteTo.Console(new JsonFormatter()))
                         .UseStartup<Startup>()
                         .Build()
                         .RunAsync();
        }
    }
}
