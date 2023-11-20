using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace JourneyJoy.ExternalAPI
{
    public class TripAdvisorAPI : BaseAPI
    {
        #region Constructors

        public TripAdvisorAPI(string APIKey, bool isEnabled) : base(APIKey, isEnabled)
        {
        }

        #endregion

        #region Public methods
        public async Task<RestResponse> SearchLocations(string searchQuery)
        {
            return await MakeGETCall($"https://api.content.tripadvisor.com/api/v1/location/search?key={APIKey}&searchQuery={searchQuery}&language=en");
        }
        #endregion
    }
}
