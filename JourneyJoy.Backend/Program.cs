using JourneyJoy.Backend.Options;
using JourneyJoy.Model.Database;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using JourneyJoy.Contracts;
using JourneyJoy.Repository;
using JourneyJoy.Utils.Security.HashAlgorithms;
using JourneyJoy.Utils.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using JourneyJoy.Utils.Security.Tokens;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JourneyJoy.ExternalAPI;
using FluentAssertions.Common;
using Microsoft.Extensions.Options;
using System.Reflection;

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
            builder.Services.AddScoped<IExternalApiService, ExternalAPIService>(x =>
            {
                return (new ExternalAPIService(new ExternalAPIOptions
                {
                    IsTripAdvisorAPIEnabled = builder.Configuration.GetSection("Backend").Get<AppOptions>().IsTripAdvisorAPIEnabled,
                    TripAdvisorAPIKey = builder.Configuration.GetSection("Backend").Get<AppOptions>().TripAdvisorAPIKey,
                    IsGoogleAPIEnabled = builder.Configuration.GetSection("Backend").Get<AppOptions>().IsGoogleAPIEnabled,
                    GoogleAPIKey = builder.Configuration.GetSection("Backend").Get<AppOptions>().GoogleAPIKey
                }));
            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtTokenHelper.User, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration.GetSection("Backend").Get<AppOptions>().JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey
                   (Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Backend").Get<AppOptions>().JwtKey)),
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    AudienceValidator = JwtTokenHelper.AudiencesValidator
                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddSwaggerGen(options =>
            {
                var basePath = AppContext.BaseDirectory;
                var fileName = typeof(Program).GetTypeInfo().Assembly.GetName().Name + ".xml";
                var commentsXmlPath = Path.Combine(basePath, fileName);
                options.IncludeXmlComments(commentsXmlPath, true);
            });
            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();


            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();

            app.Run();
        }
    }
}


