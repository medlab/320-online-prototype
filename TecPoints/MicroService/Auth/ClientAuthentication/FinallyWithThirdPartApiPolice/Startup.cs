using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace ClientAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ClientAuthentication", Version = "v1" });
            });

            //TODO
            //warning should config from environment variable, or config file section
            services.AddSingleton<ITokenService>(new CommonTokenService("client","secret", "http://127.0.0.1:4444"));

            #region Test Base On Hydra OAuth&OIDC server
            // ref https://www.ory.sh/hydra/docs/5min-tutorial/
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "http://127.0.0.1:4444";
                    //options.AutomaticAuthenticate = true;
                    options.RequireHttpsMetadata = false;
                    //options.SecurityTokenValidators.Add(new JwtSecurityTokenHandler());
                    options.TokenValidationParameters = new TokenValidationParameters{
                        ValidateAudience = false,
                    };
                });
            #endregion

            #region Test Base On Identity OAuth&OIDC server
            // ref https://github.com/IdentityServer/IdentityServer4/tree/main/samples/Quickstarts/1_ClientCredentials
            // services.AddAuthentication("Bearer")
            //     .AddJwtBearer("Bearer", options =>
            //     {
            //         options.Authority = "https://localhost:5001";
            //         
            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateAudience = false
            //         };
            //     });
            #endregion
            
            services.AddAuthorization(options =>{
                
                #region check by client_id
                options.AddPolicy("isclient", policy =>{
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("client_id", "my-client");
                });
                
                //this works too
                // options.AddPolicy("isclient", policy =>{
                //     policy.RequireAssertion(context =>
                //     {
                //         var user=context.User;
                //         Console.WriteLine("context.User.Identity.IsAuthenticated: " + context.User.Identity.IsAuthenticated);
                //         return context.User.HasClaim("client_id", "my-client");
                //     });
                // });
                #endregion

                #region check by api scope
                options.AddPolicy("apiscope", policy =>{
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "apiscope");
                });

                // this should works too
                // options.AddPolicy("apiscope", policy =>{
                //     policy.RequireAssertion(context =>{
                //         var user=context.User;
                //         Console.WriteLine("context.User.Identity.IsAuthenticated: " + context.User.Identity.IsAuthenticated);
                //         return context.User.HasClaim("scope", "apiscope");
                //     });
                // });
                #endregion
                
                #region custom assert demo
                options.AddPolicy("custom", policy =>{
                    policy.RequireAssertion(context =>
                    {
                        var user=context.User;
                        Console.WriteLine("context.User.Identity.IsAuthenticated: " + context.User.Identity.IsAuthenticated);
                        //return context.User.HasClaim("scope", "apiscope");
                        return true;
                    });
                });
                #endregion
                
                #region add middleware custom claim check
                options.AddPolicy("customcheckpassed", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                    {
                        return context.User.Claims.Any(c => c.Value == "customcheckpassed");
                    });
                });
                #endregion
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClientAuthentication v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.Use(async (context, next) =>    
            {
                if((bool)context.User?.Identity.IsAuthenticated){
                    var user = context.User.Identity;
                    var token = await CommonTokenService.Instance.GetToken();
                    //TODO access service and check result 
                    //TODO do something by thirdparty check
                    var check_result = true;
                    var i = new ClaimsIdentity();
                    i.AddClaim(new Claim("str","allowd_client"));
                    context.User.AddIdentity(i);
                }
                await next();
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
