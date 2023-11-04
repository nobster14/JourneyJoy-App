using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Testing;
using JourneyJoy.IntegrationTests.WebApplication;
using JourneyJoy.Backend;
using JourneyJoy.Backend.Options;

namespace JourneyJoy.IntegrationTests.Services
{
    public class AzureAppConfigurationTests
    {
        private WebApplicationFactory<Program>? application;
        private IOptions<AppOptions>? service;

        [SetUp]
        public void Setup()
        {
            application = ApplicationFactory.GetFullApplication<Program>();
            service = application
                .Services
                .GetService<IOptions<AppOptions>>();
        }

        [Test]
        public void AzureConfiguration_ShouldNotBeNull()
        {
            service.Should().NotBeNull();
        }

        [Test]
        public void AzureConfiguration_ShouldHaveEveryField()
        {
            var options = service!.Value;

            var properties = options.GetType()
                .GetProperties()
                .Select(prop => prop.GetValue(options))
                .ToList();

            properties.Should().AllSatisfy(prop => prop.Should().NotBeNull());
        }
    }
}
