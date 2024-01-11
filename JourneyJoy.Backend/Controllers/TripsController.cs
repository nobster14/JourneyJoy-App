using JourneyJoy.Algorithm.Algorithms;
using JourneyJoy.Backend.Options;
using JourneyJoy.Backend.Validation;
using JourneyJoy.Contracts;
using JourneyJoy.ExternalAPI;
using JourneyJoy.Model.Database.Tables;
using JourneyJoy.Model.DTOs;
using JourneyJoy.Model.DTOs.ExternalAPI.TripAdvisor;
using JourneyJoy.Model.ModelClassesSerializers;
using JourneyJoy.Model.Requests;
using JourneyJoy.Utils.Extensions;
using JourneyJoy.Utils.Security.HashAlgorithms;
using JourneyJoy.Utils.Security.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace JourneyJoy.Backend.Controllers
{
    [Route("trips")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtTokenHelper.User)]
    public class TripsController : ControllerBase
    {
        #region Fields

        private AppOptions settings;
        private IRepositoryWrapper repositoryWrapper;
        private IExternalApiService externalApiService;
        #endregion

        #region Constructors
        public TripsController(IOptions<AppOptions> settings, IRepositoryWrapper repositoryWrapper, IExternalApiService externalApiService)
        {
            this.settings = settings.Value;
            this.repositoryWrapper = repositoryWrapper;
            this.externalApiService = externalApiService;
        }
        #endregion

        #region Get methods
        // GET trips
        /// <summary>
        /// Get trips for user. Note dla Madzi: Jeżeli chodzi o trasę to na liście nie będzie atrakcji, która jest punktem startowym. Zakładamy, że po zakończeniu każdego dnia wracamy i wychodzimy z tego punktu.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripDTO[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrips([FromQuery] TakeSkipRequest request)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            if (user.UserTrips == null || user.UserTrips.Count == 0)
                return Ok();

            return Ok(this.repositoryWrapper.TripsRepository.GetByIds(user.UserTrips.Select(it => it.Id)).Select(it => TripDTO.FromDatabaseTrip(it)));
        }

        // GET trips/attractions
        /// <summary>
        /// Get attractions from TripAdvisor.
        /// </summary>
        /// <param name="latLong">Latitude/Longitude pair to scope down the search around a specifc point - eg. "42.3455,-71.10767"</param>
        /// <param name="name">Name of location</param>
        /// <returns></returns>
        [HttpGet("attractions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripAdvisorAttractionDTO[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTripAdvisorAttractions([FromQuery(Name = "name")][Required] string name, [FromQuery(Name = "latLong")] string? latLong)
        {
            var returnData = externalApiService.TripAdvisorAPI.SearchLocations(name, latLong).Result;

            /// API jest wyłączone w konfiguracji
            if (returnData == null)
                return NotFound("API is disabled - contact with administrator.");

            return Ok(returnData);
        }

        /*
         Wyłączone - robimy wewnętrznie w metodzie do dodawania atrakcji
         */
        //// GET trips/attractions/photos/{tripAdvisorLocationId}
        ///// <summary>
        ///// Get photos for attractionId from TripAdvisor. (This request uses TripAdvisor APIKey limit(5000 request a month))
        ///// </summary>
        ///// <param name="tripAdvisorLocationId">Id of TripAdvisor Location</param>
        ///// <returns></returns>
        //[HttpGet("attractions/photos/{tripAdvisorLocationId}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripAdvisorPhotoResponseDTO[]))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult GetTripAdvisorAttractionsPhotos(string tripAdvisorLocationId)
        //{
        //    var returnData = externalApiService.TripAdvisorAPI.GetPhotoForTripAdvisorLocation(tripAdvisorLocationId).Result;

        //    /// API jest wyłączone w konfiguracji
        //    if (returnData == null)
        //        return NotFound("API is disabled - contact with administrator.");

        //    return Ok(returnData);
        //}

        //// GET trips/attractions/details/{tripAdvisorLocationId}
        ///// <summary>
        ///// Get details for attractionId from TripAdvisor (Służy do pobierania godzin otwarcia). (This request uses TripAdvisor APIKey limit(5000 request a month))
        ///// </summary>
        ///// <param name="tripAdvisorLocationId">Id of TripAdvisor Location</param>
        ///// <returns></returns>
        //[HttpGet("attractions/details/{tripAdvisorLocationId}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripAdvisorDetailsResponseDTO))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult GetTripAdvisorAttractionsDetails(string tripAdvisorLocationId)
        //{
        //    var returnData = externalApiService.TripAdvisorAPI.GetDetailsForLocation(tripAdvisorLocationId).Result;

        //    /// API jest wyłączone w konfiguracji
        //    if (returnData == null)
        //        return NotFound("API is disabled - contact with administrator.");

        //    return Ok(returnData);
        //}

        #endregion

        #region Patch methods
        /// <summary>
        /// Set attraction as start point.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{tripId}/{attractionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SetAttractionAsStartPoint(Guid tripId, Guid attractionId)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            var trip = this.repositoryWrapper.TripsRepository.GetById(tripId);
            if (trip == null)
                return NotFound("Trip with given Id does not exists.");

            if (trip.Attractions == null)
                return NotFound("Attraction with calling Id not found in attraction list");

            var attractionToRemoveStartPoint = trip.Attractions.FirstOrDefault(attraction => attraction.IsStartPoint == true);
            if (attractionToRemoveStartPoint != null)
            {
                attractionToRemoveStartPoint.IsStartPoint = false;
                repositoryWrapper.AttractionRepository.Update(attractionToRemoveStartPoint);
            }

            var attractionToUpdate = trip.Attractions.FirstOrDefault(attraction => attraction.Id == attractionId);
            if (attractionToUpdate == null)
                return NotFound("Attraction with calling Id not found in attraction list");

            attractionToUpdate.IsStartPoint = true;
            this.repositoryWrapper.AttractionRepository.Update(attractionToUpdate);

            repositoryWrapper.Save();

            return Ok();
        }
        #endregion

        #region Post methods
        // POST trips
        /// <summary>
        /// Create Trip.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateTrip([FromBody] CreateTripRequest request)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");
            var tripId = Guid.NewGuid();

            var newTrip = new Trip()
            {
                Description = request.Description,
                Name = request.Name,
                Photo = request.Picture,
                Id = tripId,
                UserId = userId
            };

            if (user.UserTrips == null)
                user.UserTrips = new List<Trip>() { newTrip };
            else
                user.UserTrips.Add(newTrip);

            this.repositoryWrapper.TripsRepository.Create(newTrip);
            this.repositoryWrapper.UserRepository.Update(user);

            repositoryWrapper.Save();

            return Ok();
        }

        // POST trips/editRoute/{tripId}
        /// <summary>
        /// Edit Route Order.
        /// </summary>
        /// <param name="request">.</param>
        /// <returns></returns>
        [HttpPost("editRoute/{tripId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EditRouteForTrip(Guid tripId, [FromBody] Guid[][] attractionsInOrder)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            var trip = this.repositoryWrapper.TripsRepository.GetById(tripId);
            if (trip == null)
                return NotFound("Trip with given Id does not exists.");

            if (trip.Route == null)
                return NotFound("Route for trip does not exists");

            trip.Route.SerializedAttractionsIds = BaseObjectSerializer<Guid[][]>.Serialize(attractionsInOrder);

            this.repositoryWrapper.RouteRepository.Update(trip.Route);

            repositoryWrapper.Save();

            return Ok();
        }

        // POST trips/edit/{tripId}
        /// <summary>
        /// Edit trip.
        /// </summary>
        /// <param name="request">Edit request. If value in request is empty it will be not edited.</param>
        /// <returns></returns>
        [HttpPost("edit/{tripId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EditTrip(Guid tripId, [FromBody] CreateTripRequest request)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            var trip = this.repositoryWrapper.TripsRepository.GetById(tripId);
            if (trip == null)
                return NotFound("Trip with given Id does not exists.");


            request.EditTripFromRequest(trip);

            this.repositoryWrapper.TripsRepository.Update(trip);

            repositoryWrapper.Save();

            return Ok();
        }

        // POST trips/edit/{tripId}
        /// <summary>
        /// Edit attraction.
        /// </summary>
        /// <param name="request">Edit request. If value in request is empty it will be not edited. LocationType must be given.</param>
        /// <returns></returns>
        [HttpPost("edit/{tripId}/{attractionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EditTrip(Guid tripId, Guid attractionId, [FromBody] CreateAttractionRequest request)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            var trip = this.repositoryWrapper.TripsRepository.GetById(tripId);
            if (trip == null)
                return NotFound("Trip with given Id does not exists.");

            if (trip.Attractions == null)
                return NotFound("Attraction with calling Id not found in attraction list");

            var attractionToEdit = trip.Attractions.FirstOrDefault(attraction => attraction.Id == attractionId);
            if (attractionToEdit == null)
                return NotFound("Attraction with calling Id not found in attraction list");

            var attractionToEdit2 = this.repositoryWrapper.AttractionRepository.GetById(attractionId);
            if (attractionToEdit2 == null)
                return NotFound("Attraction not found in repository");

            request.EditAttractionFromRequest(attractionToEdit2);

            this.repositoryWrapper.AttractionRepository.Update(attractionToEdit2);

            repositoryWrapper.Save();

            return Ok();
        }

        // POST trips/route/{tripId}
        /// <summary>
        /// Create route for trip.
        /// </summary>
        /// <param name="request">Number of Days - długość wycieczki. StartDay - dzień tygodnia pierwszego dnia(0 - poniedziałek, 6-niedziela)</param>
        /// <returns></returns>
        [HttpPost("route/{tripId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult  CreateRouteForTrip(Guid tripId, [FromBody] CreateRouteRequest request)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            var trip = this.repositoryWrapper.TripsRepository.GetById(tripId);
            if (trip == null)
                return NotFound("Trip with given Id does not exists.");

            if (trip.Attractions == null  || trip.Attractions.Count == 0)
                return NotFound("There is no attractions to create route.");

            if (!trip.Attractions.Any(it => it.IsStartPoint == true))
                return NotFound("There is no start point.");

            if (trip.Attractions.Count < 3)
                return NotFound("There has to be minimum 3 attractions to create a route.");

            var calculatedRoute = GeneticAlgorithm.FindBestRoute(new Algorithm.Models.AlgorithmInformation(
                trip.Attractions.Select(it => AttractionDTO.FromDatabaseAttraction(it)).ToList(),
                externalApiService.GoogleMapsAPI.GetDistanceMatrixForAttraction(trip.Attractions.Select(it => AttractionDTO.FromDatabaseAttraction(it))).Result,
                trip.Attractions.ToList().FindIndex(it => it.IsStartPoint == true),
                request.NumberOfDays,
                request.StartDay));


            var routeToSave = new Model.Database.Tables.Route()
            {
                SerializedAttractionsIds = BaseObjectSerializer<Guid[][]>.Serialize(RouteDTO.CreateAttractionsInOrder(trip.Attractions.Select(it => AttractionDTO.FromDatabaseAttraction(it)).ToList(), calculatedRoute)),
                StartDay = request.StartDay,
                StartPointAttractionId = trip.Attractions.First(it => it.IsStartPoint).Id,
                TripId = trip.Id,
                NumberOfDays = request.NumberOfDays,
            };

            trip.Route = routeToSave;

            this.repositoryWrapper.TripsRepository.Update(trip);
            this.repositoryWrapper.Save();
            
            return Ok();
        }



        // POST trips/{tripId}
        /// <summary>
        /// Add attraction to trip.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{tripId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddAttractionToTrip(Guid tripId, [FromBody] CreateAttractionRequest request)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            var trip = this.repositoryWrapper.TripsRepository.GetById(tripId);
            if (trip == null)
                return NotFound("Trip with given Id does not exists.");

            var attraction = new Attraction()
            {
                Description = request.Description,
                TripId = trip.Id,
                Location = LocationDTO.ToDatabaseLocation(request.Location),
                LocationType = request.LocationType,
                Name = request.Name,
                Photo = request.Photo,
                TimeNeeded = request.TimeNeeded,
                IsUrl = false,
                IsStartPoint = request.IsStartPoint,
                OpenHours = BaseObjectSerializer<string[][]>.Serialize(request.OpenHours),
                Prices = BaseObjectSerializer<double[]>.Serialize(request.Prices)
            };


            if (!request.TripAdvisorLocationId.IsNullOrEmpty())
            {
                var TripAdvisorDetailsResponse = externalApiService.TripAdvisorAPI.GetDetailsForLocation(request.TripAdvisorLocationId).Result;
                var TripAdvisorPhotosResponse = externalApiService.TripAdvisorAPI.GetPhotoForTripAdvisorLocation(request.TripAdvisorLocationId).Result;

                attraction.Location.Latitude = TripAdvisorDetailsResponse.Latitude;
                attraction.Location.Longitude = TripAdvisorDetailsResponse.Longitude;
                if (TripAdvisorDetailsResponse.Hours != null)
                    attraction.OpenHours = BaseObjectSerializer<string[][]>.Serialize(TripAdvisorDetailsResponse.Hours.Periods.OrderBy(it => it.Open.Day).Select(it => new string[] { it.Open.Time, it.Close.Time }).ToArray());
                attraction.Photo = TripAdvisorPhotosResponse.First().Images.Thumbnail.Url;
                attraction.IsUrl = true;
            }

            if (attraction.Location.Longitude == 0 && attraction.Location.Latitude == 0)
            {
                var googleResponse = externalApiService.GoogleMapsAPI.ConvertAdressToLatLong(attraction.Location.Street1, attraction.Location.Country, attraction.Location.City).Result;
                if (googleResponse != null && googleResponse.Results != null && googleResponse.Results.Count() > 0)
                {
                    attraction.Location.Longitude = googleResponse.Results.First().Geometry.Location.Lng;
                    attraction.Location.Latitude = googleResponse.Results.First().Geometry.Location.Lat;
                }
            }


            if (trip.Attractions == null)
                trip.Attractions = new List<Attraction>() { attraction };
            else
                trip.Attractions.Add(attraction);

            this.repositoryWrapper.AttractionRepository.Create(attraction);
            this.repositoryWrapper.TripsRepository.Update(trip);

            repositoryWrapper.Save();

            return Ok();
        }
        #endregion

        #region Remove methods
        // DELETE trips/{tripId}
        /// <summary>
        /// Remove trip.
        /// </summary>
        /// <param name="tripId">Removed trip Id</param>
        /// <returns></returns>
        [HttpDelete("{tripId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveTrip(Guid tripId)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            var tripToRemove = user.UserTrips.FirstOrDefault(trip => trip.Id == tripId);
            if (tripToRemove == null)
                return NotFound("Calling user does not have this trip.");

            this.repositoryWrapper.TripsRepository.Delete(tripToRemove);

            user.UserTrips.Remove(tripToRemove);
            this.repositoryWrapper.UserRepository.Update(user);

            repositoryWrapper.Save();

            return Ok();
        }

        // DELETE trips/{tripId}//{attractionId}
        /// <summary>
        /// Remove attraction from Trip.
        /// </summary>
        /// <param name="tripId">Trip Id of attraction</param>
        /// <param name="attractionId">Id of attraction</param>
        /// <returns></returns>
        [HttpDelete("{tripId}/{attractionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveAttractionFromTrip(Guid tripId, Guid attractionId)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            var trip = this.repositoryWrapper.TripsRepository.GetById(tripId);
            if (trip == null)
                return NotFound("Trip with given Id does not exists.");

            if (trip.Attractions == null)
                return NotFound("Attraction with calling Id not found in attraction list");

            var attractionToRemove = trip.Attractions.FirstOrDefault(attraction => attraction.Id == attractionId);
            if (attractionToRemove == null)
                return NotFound("Attraction with calling Id not found in attraction list");

            trip.Attractions.Remove(attractionToRemove);
            this.repositoryWrapper.TripsRepository.Update(trip);

            var attractionToRemove2 = this.repositoryWrapper.AttractionRepository.GetById(attractionId);
            if (attractionToRemove2 == null)
                return NotFound("Trip with calling Id not found");

            this.repositoryWrapper.AttractionRepository.Delete(attractionToRemove2);
            repositoryWrapper.Save();

            return Ok();
        }

        // DELETE trips/route/{tripId}
        /// <summary>
        /// Remove route from trip.
        /// </summary>
        /// <param name="tripId">Trip Id of attraction</param>
        /// <returns></returns>
        [HttpDelete("route/{tripId}/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveRouteFromTrip(Guid tripId)
        {
            var userId = this.GetCallingUserId();
            var user = this.repositoryWrapper.UserRepository.GetById(userId);
            if (user == null)
                return NotFound("User does not exists.");

            var trip = this.repositoryWrapper.TripsRepository.GetById(tripId);
            if (trip == null)
                return NotFound("Trip with given Id does not exists.");
            if (trip.Route == null)
                return NotFound("Trip does not have a route.");

            this.repositoryWrapper.RouteRepository.Delete(trip.Route);

            trip.Route = null;

            this.repositoryWrapper.TripsRepository.Update(trip);
            repositoryWrapper.Save();

            return Ok();
        }
        #endregion
    }
}
