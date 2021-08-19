using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Test.Bernd.Data.Database;

namespace Test.Bernd.App.Config
{
    public static class DatabaseConfig
    {
        public static void ConfigureDatabase(this IServiceCollection services)
        {
            services.AddDbContextPool<DatabaseContext>((sp, options) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                options.UseNpgsql(configuration.GetConnectionString(nameof(DatabaseContext)),
                    o => o.CommandTimeout(60));
            });
        }

        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            appContext.Database.Migrate();

            return host;
        }
    }
}
