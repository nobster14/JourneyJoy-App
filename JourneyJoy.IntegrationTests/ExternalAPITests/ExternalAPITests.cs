using JourneyJoy.Backend;
using JourneyJoy.Backend.Options;
using JourneyJoy.ExternalAPI;
using JourneyJoy.IntegrationTests.WebApplication;
using JourneyJoy.Model.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.IntegrationTests.ExternalAPITests
{
    public class ExternalAPITests
    {
        #region Private Fields

        private WebApplicationFactory<Program>? application;
        private IExternalApiService? externalApiService;
        private string? LocationSearchQuery;
        private string? GooglePlaceId;
        private AppOptions? options;

        #endregion

        [SetUp]
        public void Setup()
        {
            application = ApplicationFactory.GetFullApplication<Program>();
            options = application
                .Services
                .GetService<IOptions<AppOptions>>().Value;

            var scope = application.Services.CreateScope();

            externalApiService = scope?.ServiceProvider.GetRequiredService<IExternalApiService>();

            LocationSearchQuery = "eiffel tower";
            GooglePlaceId = "ChIJeRpOeF67j4AR9ydy_PIzPuM";
        }

        [Test]
        public async Task TestTripAdvisorAPIOk()
        {
            if (!options.IsTripAdvisorAPIEnabled)
                return;

            var tripAdvisorAPI = externalApiService.TripAdvisorAPI;

            var response = await tripAdvisorAPI.SearchLocations(LocationSearchQuery, null);

            response.Should().NotBeNull();
        }

        [Test]
        public async Task TestGoogleAPIOk()
        {
            if (!options.IsGoogleAPIEnabled)
                return;

            var googleAPI = externalApiService.GoogleMapsAPI;

            var response = await googleAPI.GetAddressForPlaceId(GooglePlaceId);

            response.Should().NotBeNull();
        }
    }
}
