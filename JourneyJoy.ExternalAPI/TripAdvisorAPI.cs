using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace JourneyJoy.ExternalAPI
{
    public class TripAdvisorAPI 
    {
        #region Private Fields
        private bool isEnabled;
        private string APIKey;
        #endregion

        #region Constructors

        public TripAdvisorAPI(string APIKey, bool isEnabled) 
        {
            this.APIKey = APIKey;
            this.isEnabled = isEnabled;
        }

        #endregion

        #region Public methods
        public async Task<RestResponse> SearchLocations(string searchQuery)
        {
            if (!isEnabled)
                return null;

            var options = new RestClientOptions($"https://api.content.tripadvisor.com/api/v1/location/search?key={APIKey}&searchQuery={searchQuery}&language=en");
            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            var response = await client.GetAsync(request);

            return response;
        }
        #endregion
    }
}
