using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs
{
    public record LocationDTO
    {
        public string? Street1 { get; set; }

        public string? Street2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? Postalcode { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        public static LocationDTO FromDatabaseLocation(Location location)
        {
            return new LocationDTO()
            {
                Street1 = location.Street1,
                Street2 = location.Street2,
                City = location.City,
                State = location.State,
                Country = location.Country,
                Postalcode = location.Postalcode,
                Address = location.Address,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Phone = location.Phone
            };
        }

        public static Location ToDatabaseLocation(LocationDTO location)
        {
            return new Location()
            {
                Street1 = location.Street1,
                Street2 = location.Street2,
                City = location.City,
                State = location.State,
                Country = location.Country,
                Postalcode = location.Postalcode,
                Address = location.Address,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Phone = location.Phone
            };
        }
    }
}
