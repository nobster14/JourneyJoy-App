using JourneyJoy.Backend;
using JourneyJoy.IntegrationTests.TestUtilites.Http;
using JourneyJoy.IntegrationTests.WebApplication;
using JourneyJoy.Model.Database;
using JourneyJoy.Model.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.IntegrationTests.ControllersTests
{
    public abstract class BaseControllerTests
    {
        protected WebApplicationFactory<Program> Application = null!;
        protected HttpClient HttpClient = null!;
        protected const string loginEndpoint = "users/login";


        [SetUp]
        public void Setup()
        {
            Application = ApplicationFactory.GetApplicationWithMockDatabase<Program, DatabaseContext>();
            HttpClient = Application.CreateClient();
            using var scope = Application.Services.CreateScope();
            using var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>()!;

            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();

            AddInitialData(databaseContext);
        }

        protected abstract void AddInitialData(DatabaseContext databaseContext);

        protected async Task SignIn(string email, string password, string path)
        {
            var request = RequestFactory.RequestMessageWithBody(path, HttpMethod.Post, new LoginUserRequest
            {
                Email = email,
                Password = password
            });

            var result = await HttpClient.SendAsync(request);
            result.Headers.TryGetValues("Set-Cookie", out var values);

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", values!.First().Split('=')[1].Split(';').First());
        }

        protected IServiceScope GetNewScope()
        {
            return Application.Services.CreateScope();
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}
