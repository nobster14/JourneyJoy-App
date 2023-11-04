using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Data.Common;

namespace JourneyJoy.IntegrationTests.WebApplication
{
    public class WebApplicationFactoryWithMockDatabase<TProgram, TDbContext> : WebApplicationFactory<TProgram>
        where TDbContext : DbContext
        where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                
                services.AddSingleton<DbConnection>(container =>
                {
                    // Guid.NewGuid() is a trick to generate different database name for each test
                    var connection = new SqliteConnection($"DataSource={Guid.NewGuid()};Mode=Memory;Cache=shared");
                    connection.Open();

                    return connection;
                });



                services.AddDbContext<TDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection, x => { });
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

                context.Database.EnsureCreated();
            });
        }
    }
}
