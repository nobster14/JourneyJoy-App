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
        public List<Guid> AttractionsNotOnRoute { get; set; }
        public RouteDTO Route { get; set; } 

        public static TripDTO FromDatabaseTrip(Trip trip)
        {
            var retTrip =  new TripDTO()
            {
                ID = trip.Id.ToString(),
                Description = trip.Description,
                UserID = trip.User.Id.ToString(),
                Attractions = trip.Attractions == null ? null : trip.Attractions.Select(it => AttractionDTO.FromDatabaseAttraction(it)).ToList(),
                Name = trip.Name,
                Picture = trip.Photo,
                Route = trip.Route == null ? null : RouteDTO.FromDatabaseRoute(trip.Route),
            };

            if (retTrip.Route != null && trip.Attractions != null)
                retTrip.AttractionsNotOnRoute = retTrip.Attractions
                    .Select(it => it.Id)
                    .Where(it => retTrip.Route.StarePointAttractionId != it && retTrip.Route.AttractionsInOrder.Any(it2 => it2.Contains(it)))
                    .ToList();

            return retTrip;
        } 
    }
}
