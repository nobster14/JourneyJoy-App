using JourneyJoy.Backend.Options;
using JourneyJoy.Model.Database;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using JourneyJoy.Contracts;
using JourneyJoy.Repository;
using JourneyJoy.Utils.Security.HashAlgorithms;
using JourneyJoy.Utils.Validation;

namespace JourneyJoy.Backend
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure logging
            builder.Logging
                .ClearProviders()
                .AddConsole()
                .AddAzureWebAppDiagnostics();

            // Add configuration
            builder.Configuration.AddAzureAppConfiguration(builder.Configuration["AzureConfigurationConnectionString"]);
            builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("Backend"));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetSection("Backend").Get<AppOptions>().DatabaseConnectionString,
                subbuilder => subbuilder.MigrationsAssembly("JourneyJoy.Backend"));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddScoped<IHashAlgorithm, BCryptAlgorithm>();
            builder.Services.AddScoped<IValidationService, ValidationService>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


