using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using DemoWebApi.Models;
using Microsoft.Extensions.FileProviders;
using DemoWebApi.Filters;
namespace DemoWebApi
{
    public class Startup
    {
        public readonly IWebHostEnvironment _hostingEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoWebApi", Version = "v1" });
            });
            services.AddRazorPages(options =>
                {
                    options.Conventions
                        .AddPageApplicationModelConvention("/StreamedSingleFileUploadPhysical",
                            model =>
                            {
                                model.Filters.Add(
                                    new GenerateAntiforgeryTokenCookieAttribute());
                                model.Filters.Add(
                                    new DisableFormValueModelBindingAttribute());
                            });
                });
            string filePath = _hostingEnvironment.ContentRootPath + "/" + Configuration.GetValue<string>("StoredFilesPath");
            // To list physical files from a path provided by configuration:
            var physicalProvider = new PhysicalFileProvider(filePath);
            services.AddSingleton<IFileProvider>(physicalProvider);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoWebApi v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });


            // app.UseEndpoints(endpoints => {
            //     endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            //     endpoints.MapRazorPages();
            // });
        }
    }
}