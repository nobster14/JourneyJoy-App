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
        public IList<Route>? OrderedRoutes { get; set; }
        public int DayLength { get; set; }
        public double TripLength { get; set; }
    }
}
