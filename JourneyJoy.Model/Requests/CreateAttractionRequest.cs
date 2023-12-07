using JourneyJoy.Model.DTOs;
using JourneyJoy.Model.Enums;
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
        public DateTime[][] OpenHours { get; set; }
        public double[] Prices { get; set; }
        public double TimeNeeded { get; set; }
    }
}
