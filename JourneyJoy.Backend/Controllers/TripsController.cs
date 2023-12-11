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
using System.ComponentModel.DataAnnotations;
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
        // GET trips/attractions/photos/{tripAdvisorLocationId}
        /// <summary>
        /// Get photos for attractionId from TripAdvisor. (This request uses TripAdvisor APIKey limit(5000 request a month))
        /// </summary>
        /// <param name="tripAdvisorLocationId">Id of TripAdvisor Location</param>
        /// <returns></returns>
        [HttpGet("attractions/photos/{tripAdvisorLocationId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripAdvisorPhotoResponseDTO[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTripAdvisorAttractionsPhotos(string tripAdvisorLocationId)
        {
            var returnData = externalApiService.TripAdvisorAPI.GetPhotoForTripAdvisorLocation(tripAdvisorLocationId).Result;

            /// API jest wyłączone w konfiguracji
            if (returnData == null)
                return NotFound("API is disabled - contact with administrator.");

            return Ok(returnData);
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

        // POST trips/{tripId}
        /// <summary>
        /// Add attraction to trip.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{tripId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                OpenHours = BaseObjectSerializer<int[][]>.Serialize(request.OpenHours),
                Prices = BaseObjectSerializer<double[]>.Serialize(request.Prices),
                Photo = request.Photo,
                TimeNeeded = request.TimeNeeded,
            };

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

        #region Get methods
        // GET trips
        /// <summary>
        /// Get trips for user.
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

        // DELETE trips/{tripId}
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
                return NotFound("Trip with calling Id not found in attraction list");

            var attractionToRemove = trip.Attractions.FirstOrDefault(attraction => attraction.Id == attractionId);
            if (attractionToRemove == null)
                return NotFound("Trip with calling Id not found in attraction list");

            trip.Attractions.Remove(attractionToRemove);
            this.repositoryWrapper.TripsRepository.Update(trip);

            var attractionToRemove2 = this.repositoryWrapper.AttractionRepository.GetById(attractionId);
            if (attractionToRemove2 == null)
                return NotFound("Trip with calling Id not found");

            this.repositoryWrapper.AttractionRepository.Delete(attractionToRemove2);
            repositoryWrapper.Save();

            return Ok();
        }
        #endregion
    }
}
