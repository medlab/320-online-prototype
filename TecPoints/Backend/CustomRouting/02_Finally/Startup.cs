using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Routing;

namespace _02_Finally
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var displayUrl = context.Request.GetDisplayUrl();
                    var helloFullPath = $"{displayUrl}hello";
                    await context.Response.WriteAsync($"To access hello route, access {helloFullPath}");
                });

                endpoints.MapGet("/hello", async context =>
                {
                    var displayUrl = context.Request.GetDisplayUrl();
                    var rootFullPath = displayUrl.Replace("hello","");
                    await context.Response.WriteAsync($"To access root, visitor {rootFullPath}");
                });
            });
        }
    }
}
