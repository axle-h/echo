using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Echo
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (_configuration.GetValue<bool>("UseForwardedHeaders"))
            {
                var options = new ForwardedHeadersOptions
                              {
                                  ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                              };
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                app.UseForwardedHeaders(options);
            }

            app.Run(async ctx =>
                    {
                        var logger = ctx.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Request");
                        
                        using (var reader = new StreamReader(ctx.Request.Body))
                        {
                            var url = ctx.Request.GetDisplayUrl();
                            var headers = ctx.Request.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value.ToArray()));
                            var body = await reader.ReadToEndAsync();

                            logger.LogInformation("{url} {headers} {body}", url, headers, body);
                            await ctx.Response.WriteAsync(JsonConvert.SerializeObject(new { url, headers, body }, Formatting.Indented));
                        }
                    });
        }
    }
}
