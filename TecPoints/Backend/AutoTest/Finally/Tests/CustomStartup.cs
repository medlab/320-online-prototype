using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Items;
namespace Tests
{
    public class CustomStartup : Startup
    {
        public CustomStartup(IConfiguration configuration) : base(configuration) { }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<ItemsContext>(
                conext => conext.UseSqlite(Tests.SqliteInMemoryItemsControllerTest.CreateInMemoryDatabase()));
        }
    }
}