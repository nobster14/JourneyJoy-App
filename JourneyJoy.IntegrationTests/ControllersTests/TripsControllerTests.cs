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
        private string attractionAdress = "attraction test adress";
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

        private CreateRouteRequest GetCreateRouteRequest() => new CreateRouteRequest()
        {
            NumberOfDays = 7,
            StartDay = 0
        };

        private CreateAttractionRequest GetCreateAttractionRequest()
        {
            var ret = new CreateAttractionRequest()
            {
                Description = "trip description",
                Name = "trip name",
                Photo = "Trip picture",
                Location = new LocationDTO()
                {
                    Address = attractionAdress
                },
                Prices = Enumerable.Range(0, 7).Select(it => (double)it).ToArray()
            };

            string[][] dateTimeArray = new string[7][];


            for (int i = 0; i < 7; i++)
            {
                dateTimeArray[i] = new string[2];
                for (int j = 0; j < 2; j++)
                {
                    // Tak np. zapisujemy godzinę 19:00
                    dateTimeArray[i][j] = "1900";
                }
            }

            ret.OpenHours = dateTimeArray;

            return ret;
        }

        private CreateAttractionRequest GetCreateAttractionRequest1()
        {
            var ret = new CreateAttractionRequest()
            {
                Description = "trip description",
                Name = "trip name",
                Photo = "Trip picture",
                Location = new LocationDTO()
                {
                    Address = "Koszykowa 75 Warszawa",
                    City = "Warszawa",
                    Street1 = "Koszykowa 75",
                    Country = "Polska"
                },
                Prices = Enumerable.Range(0, 7).Select(it => (double)it).ToArray()
            };

            string[][] dateTimeArray = new string[7][];


            for (int i = 0; i < 7; i++)
            {
                dateTimeArray[i] = new string[2];

                dateTimeArray[i][0] = "0000";
                dateTimeArray[i][1] = "2400";
            }

            ret.OpenHours = dateTimeArray;

            return ret;
        }
        private CreateAttractionRequest GetCreateAttractionRequest2()
        {
            var ret = new CreateAttractionRequest()
            {
                Description = "trip description",
                Name = "trip name",
                Photo = "Trip picture",
                Location = new LocationDTO()
                {
                    Address = "Jana Pawła 2 137 Kraków",
                    City = "Kraków",
                    Street1 = "Jana Pawła II 137",
                    Country = "Polska"
                },
                Prices = Enumerable.Range(0, 7).Select(it => (double)it).ToArray()
            };

            string[][] dateTimeArray = new string[7][];


            for (int i = 0; i < 7; i++)
            {
                dateTimeArray[i] = new string[2];

                dateTimeArray[i][0] = "0000";
                dateTimeArray[i][1] = "2400";
            }

            ret.OpenHours = dateTimeArray;

            return ret;
        }
        private CreateAttractionRequest GetCreateAttractionRequest3()
        {
            var ret = new CreateAttractionRequest()
            {
                Description = "trip description",
                Name = "trip name",
                Photo = "Trip picture",
                Location = new LocationDTO()
                {
                    Address = "Jana Pawła 2 51 Lublin",
                    City = "Lublin",
                    Street1 = "Jana Pawła II 51",
                    Country = "Polska"
                },
                Prices = Enumerable.Range(0, 7).Select(it => (double)it).ToArray(),
                IsStartPoint = true

            };

            string[][] dateTimeArray = new string[7][];


            for (int i = 0; i < 7; i++)
            {
                dateTimeArray[i] = new string[2];

                dateTimeArray[i][0] = "0000";
                dateTimeArray[i][1] = "2400";
            }

            ret.OpenHours = dateTimeArray;

            return ret;
        }
        #endregion

        #region Tests
        /// <summary>
        /// Testuje intergację modułu algorytmu z backendem
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_AlgorithmEndpoint()
        {
            await SignIn(userEmail, userPassword, "users/login");

            //Tworzymy wycieczkę
            var request = RequestFactory.RequestMessageWithBody(TripsEndpoint, HttpMethod.Post, GetCreateTripRequest());
            var response = await HttpClient.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Pobieramy wycieczkę
            var response1 = await HttpClient.GetAsync(TripsEndpoint);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response1.GetContent<TripDTO[]>();
            result?.FirstOrDefault().Should().NotBeNull();
            result.First().Name.Should().Be("trip name");


            var tripId = result.First().ID;


            //Tworzymy atrakcje
            var createAttractionRequest1 = RequestFactory.RequestMessageWithBody($"{TripsEndpoint}/{tripId}", HttpMethod.Post, GetCreateAttractionRequest1());
            var createAttractionResponse1 = await HttpClient.SendAsync(createAttractionRequest1);
            createAttractionResponse1.StatusCode.Should().Be(HttpStatusCode.OK);

            var createAttractionRequest2 = RequestFactory.RequestMessageWithBody($"{TripsEndpoint}/{tripId}", HttpMethod.Post, GetCreateAttractionRequest2());
            var createAttractionResponse2 = await HttpClient.SendAsync(createAttractionRequest2);
            createAttractionResponse2.StatusCode.Should().Be(HttpStatusCode.OK);

            var createAttractionRequest3 = RequestFactory.RequestMessageWithBody($"{TripsEndpoint}/{tripId}", HttpMethod.Post, GetCreateAttractionRequest3());
            var createAttractionResponse3 = await HttpClient.SendAsync(createAttractionRequest3);
            createAttractionResponse3.StatusCode.Should().Be(HttpStatusCode.OK);

            //Akcja utworzenia trasy
            var createRouteRequest = RequestFactory.RequestMessageWithBody($"{TripsEndpoint}/route/{tripId}", HttpMethod.Post, GetCreateRouteRequest());
            var createRouteResponse = await HttpClient.SendAsync(createRouteRequest);
            createRouteResponse.StatusCode.Should().Be(HttpStatusCode.OK);



            //Pobieramy wycieczkę
            response1 = await HttpClient.GetAsync(TripsEndpoint);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            result = await response1.GetContent<TripDTO[]>();
            result?.FirstOrDefault().Should().NotBeNull();
            result.First().Name.Should().Be("trip name");

        }



        /// <summary>
        /// Testuje metody POST, GET, REMOVE dla wycieczek oraz POST, REMOVE dla atrakcji
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_TripAndAttractionLifeCycle()
        {
            await SignIn(userEmail, userPassword, "users/login");

            //Tworzymy wycieczkę
            var request = RequestFactory.RequestMessageWithBody(TripsEndpoint, HttpMethod.Post, GetCreateTripRequest());
            var response = await HttpClient.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Pobieramy wycieczkę
            var response1 = await HttpClient.GetAsync(TripsEndpoint);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response1.GetContent<TripDTO[]>();
            result?.FirstOrDefault().Should().NotBeNull();
            result.First().Name.Should().Be("trip name");


            var tripId = result.First().ID;

            //Edytujemy wycieczkę
            var editObj = GetCreateTripRequest();
            editObj.Name = "edited name";
            var editrequest = RequestFactory.RequestMessageWithBody($"{TripsEndpoint}/edit/{tripId}", HttpMethod.Post, editObj);
            var editresponse = await HttpClient.SendAsync(editrequest);
            editresponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //Pobieramy wycieczkę
            response1 = await HttpClient.GetAsync(TripsEndpoint);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            result = await response1.GetContent<TripDTO[]>();
            result?.FirstOrDefault().Should().NotBeNull();
            result.First().Name.Should().Be("edited name");

            //Tworzymy atrakcję
            var createAttractionRequest = RequestFactory.RequestMessageWithBody($"{TripsEndpoint}/{tripId}", HttpMethod.Post, GetCreateAttractionRequest());
            var createAttractionResponse = await HttpClient.SendAsync(createAttractionRequest);
            createAttractionResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            response1 = await HttpClient.GetAsync(TripsEndpoint);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            result = await response1.GetContent<TripDTO[]>();
            result?.FirstOrDefault().Should().NotBeNull();

            result.First().ID.Should().Be(tripId);
            result.First().Attractions.Should().NotBeNull();
            result.First().Attractions.Count().Should().Be(1);
            var addedAttraction = result.First().Attractions.First();
            addedAttraction.Location.Should().NotBeNull();
            addedAttraction.Location.Address.Should().Be(attractionAdress);
            addedAttraction.OpenHours.Should().NotBeNull();
            addedAttraction.OpenHours.Count().Should().Be(7);
            addedAttraction.OpenHours[0].Count().Should().Be(2);
            addedAttraction.IsStartPoint.Should().BeFalse();

            var setAttractionAsStartPoint = await HttpClient.PatchAsync($"{TripsEndpoint}/{tripId}/{addedAttraction.Id}", null);

            response1 = await HttpClient.GetAsync(TripsEndpoint);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            result = await response1.GetContent<TripDTO[]>();
            result?.FirstOrDefault().Should().NotBeNull();
            var modifiedAsStartPoint = result.First().Attractions.First();
            modifiedAsStartPoint.IsStartPoint.Should().BeTrue();


            //Usuwamy atrakcję
            var removeAttractionResponse = await HttpClient.DeleteAsync($"{TripsEndpoint}/{tripId}/{addedAttraction.Id}");
            removeAttractionResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            response1 = await HttpClient.GetAsync(TripsEndpoint);
            response1.StatusCode.Should().Be(HttpStatusCode.OK);

            result = await response1.GetContent<TripDTO[]>();
            result?.FirstOrDefault().Should().NotBeNull();

            result.First().ID.Should().Be(tripId);
            result.First().Attractions.Count.Should().Be(0);


            using var scope = GetNewScope();
            using var context = scope.ServiceProvider.GetService<DatabaseContext>()!;
            context.Set<Attraction>().Count().Should().Be(0);

            //Usuwamy wycieczkę
            var response2 = await HttpClient.DeleteAsync($"{TripsEndpoint}/{tripId}");
            response2.StatusCode.Should().Be(HttpStatusCode.OK);

            var response3 = await HttpClient.GetAsync(TripsEndpoint);
            response3.StatusCode.Should().Be(HttpStatusCode.OK);
            var result2 = await response3.GetContent<TripDTO[]>();
            result2?.FirstOrDefault().Should().BeNull();

            using var scope1 = GetNewScope();
            using var context2 = scope1.ServiceProvider.GetService<DatabaseContext>()!;
            context2.Set<Trip>().Count().Should().Be(0);

            var response4 = await HttpClient.GetAsync(TripsEndpoint);
            response4.StatusCode.Should().Be(HttpStatusCode.OK);

            var result3 = await response4.GetContent<TripDTO[]>();
            result3?.FirstOrDefault().Should().BeNull();
        }
        #endregion
    }
}
