using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs
{
    public record TripDTO
    {
        public string? ID { get; set; }

        public string? UserID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public List<AttractionDTO> Attractions { get; set; }
        public static TripDTO FromDatabaseTrip(Trip trip)
        {
            return new TripDTO()
            {
                ID = trip.Id.ToString(),
                Description = trip.Description,
                UserID = trip.User.Id.ToString(),
                Attractions = trip.Attractions == null ? null : trip.Attractions.Select(it => AttractionDTO.FromDatabaseAttraction(it)).ToList(),
                Name = trip.Name,
                Picture = trip.Photo
            };
        }
    }
}
