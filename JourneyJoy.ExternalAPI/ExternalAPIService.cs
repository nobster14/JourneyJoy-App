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
        #endregion

        #region Constructors

        public ExternalAPIService(ExternalAPIOptions options)
        {
            this.options = options;
            this.tripAdvisorAPI = new TripAdvisorAPI(options.TripAdvisorAPIKey, options.IsTripAdvisorAPIEnabled);
        }


        #endregion

        #region Properties

        public TripAdvisorAPI TripAdvisorAPI => tripAdvisorAPI;

        #endregion
    }
}
