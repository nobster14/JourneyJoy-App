using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.ExternalAPI
{
    public class GoogleMapsAPI : BaseAPI
    {
        #region Constructors
        public GoogleMapsAPI(string APIKey, bool isEnabled) : base(APIKey, isEnabled)
        {
        }

        #endregion

        #region Public methods
        public async Task<RestResponse> GetAddressForPlaceId(string placeId)
        {
            return await MakeGETCall($"https://maps.googleapis.com/maps/api/geocode/json?place_id={placeId}&key={APIKey}");
        }
        #endregion
    }
}
