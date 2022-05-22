using bookshop_catalog.Infrastructure.Resilience;
using Microsoft.EntityFrameworkCore;

namespace bookshop_catalog.Extensions
{
    internal static class WebApplicationExtensions
    {
        internal static WebApplication SetupDatabaseMigrations<TContext>(this WebApplication app, Action<TContext> seeder) where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();

            var context = services.GetService<TContext>();

            try
            {
                var policy = SQLRetryPolicies.CreateDBSeedingPolicy();
                policy.Execute(() =>
                {
                    context!.Database.Migrate();
                    seeder(context);
                });
            }
            catch (Exception ex)
            {
                // TODO: Provide serilog logger message
                Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
                throw;
            }

            return app;
        }
    }
}
