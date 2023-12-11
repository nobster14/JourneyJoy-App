using JourneyJoy.Model.Database.Tables;
using JourneyJoy.Model.Enums;
using JourneyJoy.Model.ModelClassesSerializers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs
{
    public record AttractionDTO
    {
        public Guid Id { get; set; }
        public Guid TripID { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(500)]
        public string? Description { get; set; }
        public string Photo { get; set; } = null!;
        public LocationDTO Location { get; set; } = null!;

        public LocationType LocationType { get; set; }

        public string[][] OpenHours { get; set; }
        public double[] Prices { get; set; }
        public double TimeNeeded { get; set; }
        public bool IsStartPoint { get; set; }
        public static AttractionDTO FromDatabaseAttraction(Attraction attraction)
        {
            return new AttractionDTO()
            {
                Id = attraction.Id,
                Description = attraction.Description,
                Photo = attraction.Photo,
                Location = LocationDTO.FromDatabaseLocation(attraction.Location), 
                LocationType = attraction.LocationType,
                Prices = BaseObjectSerializer<double[]>.Deserialize(attraction.Prices),
                TimeNeeded = attraction.TimeNeeded,
                TripID = attraction.Trip.Id,
                Name = attraction.Name,
                OpenHours = BaseObjectSerializer<string[][]>.Deserialize(attraction.OpenHours),
                IsStartPoint = attraction.IsStartPoint,
            };
        }

        public static Attraction ToDatabaseAttraction(AttractionDTO attraction)
        {
            return new Attraction()
            {
                Id = attraction.Id,
                Description = attraction.Description,
                Photo = attraction.Photo,
                Location = LocationDTO.ToDatabaseLocation(attraction.Location),
                LocationType = attraction.LocationType,
                Prices = BaseObjectSerializer<double[]>.Serialize(attraction.Prices),
                TimeNeeded = attraction.TimeNeeded,
                Trip = new Trip() { Id = attraction.TripID },
                Name = attraction.Name,
                OpenHours = BaseObjectSerializer<string[][]>.Serialize(attraction.OpenHours),
                IsStartPoint = attraction.IsStartPoint, 
            };
        }

    }
}
