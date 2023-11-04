using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace JourneyJoy.IntegrationTests.WebApplication
{
    public static class ApplicationFactory
    {
        public static WebApplicationFactory<TProgram> GetFullApplication<TProgram>() where TProgram : class
        {
            return new WebApplicationFactory<TProgram>();
        }

        public static WebApplicationFactoryWithMockDatabase<TProgram, TDbContext> GetApplicationWithMockDatabase<TProgram, TDbContext>()
            where TProgram : class
            where TDbContext : DbContext
        {
            return new WebApplicationFactoryWithMockDatabase<TProgram, TDbContext>();
        }
    }
}
