using JourneyJoy.Backend.Options;
using JourneyJoy.Backend.Validation;
using JourneyJoy.Contracts;
using JourneyJoy.ExternalAPI;
using JourneyJoy.Model.Database.Tables;
using JourneyJoy.Model.DTOs;
using JourneyJoy.Model.DTOs.ExternalAPI.TripAdvisor;
using JourneyJoy.Model.Requests;
using JourneyJoy.Utils.Security.HashAlgorithms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace JourneyJoy.Backend.Controllers
{
    [Route("trips")]
    [ApiController]
    [Authorize]
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

            return Ok(returnData);
        }

        #endregion
    }
}
