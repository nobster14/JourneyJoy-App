﻿using JourneyJoy.Model.Algorithm;
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
using JourneyJoy.Algorithm.Algorithms;

namespace JourneyJoy.ExternalAPI
{
    public class GoogleMapsAPI : BaseAPI
    {
        private const int GoogleMapsMatrixRowsLimit = 10;
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

        public async Task<int[][]> GetDistanceMatrixForAttraction(IEnumerable<AttractionDTO> attractions)
        {
            /// Limit Google Maps Distance API to macierz 10x10
            var destinasionsOriginsString = attractions.Take(GoogleMapsMatrixRowsLimit).Select(it => $"{it.Location.Latitude.ToString(CultureInfo.InvariantCulture)}%2C{it.Location.Longitude.ToString(CultureInfo.InvariantCulture)}").Aggregate((it1, it2) => $"{it1}%7C{it2}");

            var response =  await MakeGETCall($"https://maps.googleapis.com/maps/api/distancematrix/json?destinations={destinasionsOriginsString}&origins={destinasionsOriginsString}&key={APIKey}");

            /// API jest wyłączone w konfiguracji
            if (response == null)
                return null;

            var deserializedResponse = JsonConvert.DeserializeObject<GoogleMapsDistanceMatrixReponseDTO>(response.Content);

            var ret = new int[attractions.Count()][];
            for (int i = 0; i < attractions.Count(); i++)
                ret[i] = new int[attractions.Count()];

            foreach (var i in Enumerable.Range(0, attractions.Count()))
                foreach (var j in Enumerable.Range(0, attractions.Count()))
                {
                    DistanceDTO actualElem = null;

                    if (deserializedResponse != null && i < GoogleMapsMatrixRowsLimit && j < GoogleMapsMatrixRowsLimit && deserializedResponse.Rows[i] != null && deserializedResponse.Rows[i].Elements[j] != null)
                        actualElem = deserializedResponse.Rows[i].Elements[j];

                    if (actualElem != null && actualElem.Duration != null)
                        ret[i][j] = actualElem.Duration.Value / 60;
                    else
                        ret[i][j] = Haversine.CalculateFormula(attractions.ElementAt(i).Location.Latitude, attractions.ElementAt(i).Location.Longitude, attractions.ElementAt(j).Location.Latitude, attractions.ElementAt(j).Location.Longitude) / 180;
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
