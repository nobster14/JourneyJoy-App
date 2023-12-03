using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs
{
    public record TripDTO
    {
        public string? ID { get; }
        public string? Name { get; }
        public string? Description { get; set; }
        public string? Picture { get; }
        public List<AttractionDTO> Attractions { get; }
    }
}
