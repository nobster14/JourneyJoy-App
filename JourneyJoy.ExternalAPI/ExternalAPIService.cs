using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.ExternalAPI
{
    public class ExternalAPIService : IExternalApiService
    {
        #region Private Fields
        private ExternalAPIOptions options;
        private TripAdvisorAPI tripAdvisorAPI;
        private GoogleMapsAPI googleMapsAPI;
        #endregion

        #region Constructors

        public ExternalAPIService(ExternalAPIOptions options)
        {
            this.options = options;
            this.tripAdvisorAPI = new TripAdvisorAPI(options.TripAdvisorAPIKey, options.IsTripAdvisorAPIEnabled);
            this.googleMapsAPI = new GoogleMapsAPI(options.GoogleAPIKey, options.IsGoogleAPIEnabled);
        }


        #endregion

        #region Properties

        public TripAdvisorAPI TripAdvisorAPI => tripAdvisorAPI;

        public GoogleMapsAPI GoogleMapsAPI => googleMapsAPI;

        #endregion
    }
}
