using JourneyJoy.Model.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs
{
    public record AttractionDTO
    {
        public static AttractionDTO FromDatabaseAttraction(Attraction trip)
        {
            return new AttractionDTO();
        }

    }
}
