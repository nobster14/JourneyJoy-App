using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Database.Tables
{
    public record Trip
    {
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(5000)]
        public string? Description { get; set; }
        public string? Photo { get; set; }
        ICollection<Attraction> Attractions { get; set; } = null!;
        public Route? Route { get; set; }
    }
}
