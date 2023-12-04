using JourneyJoy.IntegrationTests.TestUtilites.Http;
using JourneyJoy.Model.Database;
using JourneyJoy.Model.Database.Tables;
using JourneyJoy.Model.DTOs;
using JourneyJoy.Model.Requests;
using JourneyJoy.Utils.Security.HashAlgorithms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Execution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.IntegrationTests.ControllersTests
{
    [TestFixture]
    public class TripsControllerTests : BaseControllerTests
    {
        #region Private fields
        private Guid userId;
        private string userEmail = null!;
        private string userPassword = null!;

        protected const string TripsEndpoint = "trips";
        #endregion


        #region Override methods
        protected override void AddInitialData(DatabaseContext databaseContext)
        {
            userId = Guid.NewGuid();
            userEmail = "testuser@mail.com";
            userPassword = "tesT@ssword123";

            var user = new User
            {
                Id = userId,
                Nickname = "testNickname",
                Email = userEmail,
                Password = new BCryptAlgorithm().Hash(userPassword),
                UserTrips = new List<Trip>()
            };
            databaseContext.Add(user);

            databaseContext.SaveChanges();
        }
        #endregion

        #region Private methods
        private CreateTripRequest GetCreateTripRequest() => new CreateTripRequest()
        {
            Description = "trip description",
            Name = "trip name",
            Picture = "Trip picture"
        };
        #endregion

        #region Tests
        [Test]
        public async Task Test_Create_Remove_Trip()
        {
            await SignIn(userEmail, userPassword, "users/login");

            var request = RequestFactory.RequestMessageWithBody(TripsEndpoint, HttpMethod.Post, GetCreateTripRequest());
            var response = await HttpClient.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var response1 = await HttpClient.GetAsync(TripsEndpoint);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response1.GetContent<TripDTO[]>();
            result?.FirstOrDefault().Should().NotBeNull();

            var tripId = result.First().ID;

            var response2 = await HttpClient.DeleteAsync($"{TripsEndpoint}/{tripId}");
            response2.StatusCode.Should().Be(HttpStatusCode.OK);

            var response3 = await HttpClient.GetAsync(TripsEndpoint);
            response3.StatusCode.Should().Be(HttpStatusCode.OK);
            var result2 = await response3.GetContent<TripDTO[]>();
            result2?.FirstOrDefault().Should().BeNull();

            using var scope = GetNewScope();
            using var context = scope.ServiceProvider.GetService<DatabaseContext>()!;
            context.Set<Trip>().Count().Should().Be(0);

            var response4 = await HttpClient.GetAsync(TripsEndpoint);
            response4.StatusCode.Should().Be(HttpStatusCode.OK);

            var result3 = await response4.GetContent<TripDTO[]>();
            result3?.FirstOrDefault().Should().BeNull();
        }
        #endregion
    }
}
