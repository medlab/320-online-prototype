using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestAuthWebapp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

[assembly: HostingStartup(typeof(TestAuthWebapp.Areas.Identity.IdentityHostingStartup))]
namespace TestAuthWebapp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<TestAuthWebappIdentityDbContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("TestAuthWebappIdentityDbContextConnection")));
                services.AddDefaultIdentity<TestAuthWebappUser>(
                    options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<TestAuthWebappIdentityDbContext>();
                services.AddAuthorization(options =>
                {
                    options.FallbackPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                });
            });
        }
    }
}