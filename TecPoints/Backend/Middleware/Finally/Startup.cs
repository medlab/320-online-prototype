using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Finally
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestCulture();

            // deal with the response, previously we had set the CultureInfo in RequestCultureMiddleware
            app.Run(async (context) =>
            {
                string LocalHello = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol.ToString();

                await context.Response.WriteAsync("The LocalCurrencySymbol is" + " \"" + LocalHello + "\"");
            });
        }
    }
}