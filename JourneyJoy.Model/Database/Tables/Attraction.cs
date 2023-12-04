using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Database.Tables
{
    public record Attraction
    {
        public Guid Id { get; set; }
        public Trip Trip { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(500)]
        public string? Description { get; set; }
        public string Photo { get; set; } = null!;
        public Location Location { get; set; } = null!;

    }
}
