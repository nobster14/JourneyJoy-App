using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.Database.Tables
{
    public record Route
    {
        public Guid Id { get; set; }
        public int StartDay { get; set; }

        public string? SerializedAttractionsIds {  get; set; }
    }

}
