using JourneyJoy.Model.Algorithm;
using JourneyJoy.Model.DTOs;
using JourneyJoy.Model.DTOs.ExternalAPI.TripAdvisor;
using JourneyJoy.Model.DTOs.ExternalAPI;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JourneyJoy.Model.DTOs.ExternalAPI.GoogleMaps;
using System.Globalization;

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
            var response = await MakeGETCall($"https://maps.googleapis.com/maps/api/geocode/json?place_id={placeId}&key={APIKey}");

            /// API jest wyłączone w konfiguracji
            if (response == null)
                return null;

            return response;
        }

        public async Task<DistanceTimeDTO[][]> GetDistanceMatrixForAttraction(IEnumerable<AttractionDTO> attractions)
        {
            var destinasionsOriginsString = attractions.Select(it => $"{it.Location.Latitude.ToString(CultureInfo.InvariantCulture)}%2C{it.Location.Longitude.ToString(CultureInfo.InvariantCulture)}").Aggregate((it1, it2) => $"{it1}%7C{it2}");

            var response =  await MakeGETCall($"https://maps.googleapis.com/maps/api/distancematrix/json?destinations={destinasionsOriginsString}&origins={destinasionsOriginsString}&key={APIKey}");

            /// API jest wyłączone w konfiguracji
            if (response == null)
                return null;

            var deserializedResponse = JsonConvert.DeserializeObject<GoogleMapsDistanceMatrixReponseDTO>(response.Content);

            var ret = new DistanceTimeDTO[attractions.Count()][];
            for (int i = 0; i < attractions.Count(); i++)
                ret[i] = new DistanceTimeDTO[attractions.Count()];
            
            foreach (var i in Enumerable.Range(0, attractions.Count()))
                foreach (var j in Enumerable.Range(0, attractions.Count()))
                {
                    var actualElem = deserializedResponse.Rows[i].Elements[j];

                    ret[i][j] = new DistanceTimeDTO()
                    {
                        Distance = actualElem.Distance.Value,
                        Time = actualElem.Duration.Value,
                    };
                }

            return ret;
        }

        public async Task<GoogleMapsGeocodingResponseDTO> ConvertAdressToLatLong(string street, string country, string city)
        {

            var response = await MakeGETCall($"https://maps.googleapis.com/maps/api/geocode/json?address={street}%20{city}%20{country}&key={APIKey}");

            /// API jest wyłączone w konfiguracji
            if (response == null)
                return null;

            var deserializedResponse = JsonConvert.DeserializeObject<GoogleMapsGeocodingResponseDTO>(response.Content);



            return deserializedResponse;
        }
        #endregion
    }
}
