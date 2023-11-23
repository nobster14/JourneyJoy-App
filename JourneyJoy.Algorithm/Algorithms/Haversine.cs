using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms
{
    public static class Haversine
    {
        public static float CalculateFormula(float lat1, float lon1, float lat2, float lon2)
        {
            const float r = 6378100;

            var sdLat = Math.Sin((lat2 - lat1) / 2);
            var sdLon = Math.Sin((lon2 - lon1) / 2);
            var q = sdLat * sdLat + Math.Cos(lat1) * Math.Cos(lat2) * sdLon * sdLon;
            var d = 2 * r * Math.Asin(Math.Sqrt(q));

            return (float)d;
        }
    }
}
