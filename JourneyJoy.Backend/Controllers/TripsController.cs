using JourneyJoy.Backend.Options;
using JourneyJoy.Contracts;
using JourneyJoy.ExternalAPI;
using JourneyJoy.Utils.Security.HashAlgorithms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JourneyJoy.Backend.Controllers
{
    [Route("trips")]
    [ApiController]
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
    }
}
