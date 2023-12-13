using JourneyJoy.Model.Database.Tables;
using JourneyJoy.Model.DTOs;
using JourneyJoy.Model.Enums;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Requests
{
    public record CreateAttractionRequest
    {
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(500)]
        public string? Description { get; set; }
        public string Photo { get; set; } = null!;
        public LocationDTO Location { get; set; } = null!;

        public LocationType LocationType { get; set; }

        /// <summary>
        /// Array 7x2(2 rows, 7 columns) for each date start and end hour in format ISO 8601
        /// </summary>
        public int[][] OpenHours { get; set; }
        public double[] Prices { get; set; }
        public double TimeNeeded { get; set; }

        public void EditAttractionFromRequest(Attraction attraction)
        {
            if (!Name.IsNullOrEmpty())
                attraction.Name = Name;
            if (!Description.IsNullOrEmpty())
                attraction.Description = Description;
            if (!Photo.IsNullOrEmpty())
                attraction.Photo = Photo;

            attraction.LocationType = LocationType;

            if (Location != null)
            {
                if (!Location.Address.IsNullOrEmpty())
                    attraction.Location.Address = Location.Address;
                if (!Location.City.IsNullOrEmpty())
                    attraction.Location.City = Location.City;
                if (!Location.State.IsNullOrEmpty())
                    attraction.Location.State = Location.State;
                if (!Location.Street1.IsNullOrEmpty())
                    attraction.Location.Street1 = Location.Street1;
                if (!Location.Phone.IsNullOrEmpty())
                    attraction.Location.Phone = Location.Phone;
                if (!Location.Postalcode.IsNullOrEmpty())
                    attraction.Location.Postalcode = Location.Postalcode;
                if (Location.Latitude != default(double))
                    attraction.Location.Latitude = Location.Latitude;
                if (Location.Longitude != default(double))
                    attraction.Location.Longitude = Location.Longitude;
                if (Location.Country.IsNullOrEmpty())
                    attraction.Location.Country = Location.Country;
            }
        }
    }
}
