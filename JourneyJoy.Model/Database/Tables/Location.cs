using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Database.Tables
{
    public record Location
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

    }
}
