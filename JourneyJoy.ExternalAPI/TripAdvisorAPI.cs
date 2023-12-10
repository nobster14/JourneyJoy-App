using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JourneyJoy.Model.DTOs.ExternalAPI;
using JourneyJoy.Model.DTOs.ExternalAPI.TripAdvisor;
using Newtonsoft.Json;
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
        public async Task<TripAdvisorAttractionDTO[]> SearchLocations(string searchQuery, string? latLong)
        {
            StringBuilder url = new StringBuilder($"https://api.content.tripadvisor.com/api/v1/location/search?key={APIKey}&searchQuery={searchQuery}&language=en");
            if (latLong != null)
                url.Append($"&latLong={latLong}");

            var res = await MakeGETCall(url.ToString());
            
            /// API jest wyłączone w konfiguracji
            if (res == null)
                return null;

            return JsonConvert.DeserializeObject<BasicJsonArray<TripAdvisorAttractionDTO[]>>(res.Content).Data;
        }

        public async Task<TripAdvisorPhotoResponseDTO[]> GetPhotoForTripAdvisorLocation(string locationId)
        {
            StringBuilder url = new StringBuilder($"https://api.content.tripadvisor.com/api/v1/location/{locationId}/photos?key={APIKey}&language=en");

            var res = await MakeGETCall(url.ToString());

            /// API jest wyłączone w konfiguracji
            if (res == null)
                return null;

            return JsonConvert.DeserializeObject<BasicJsonArray<TripAdvisorPhotoResponseDTO[]>>(res.Content).Data;
        }
        #endregion
    }
}
