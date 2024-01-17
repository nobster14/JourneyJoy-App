using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Algorithm.Algorithms
{
    public static class Haversine
    {
        /// <summary>
        /// Calculates Haversine formula from two points on Earth.
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns></returns>
        public static int CalculateFormula(double lat1, double lon1, double lat2, double lon2)
        {
            const float r = 6378100;

            var sdLat = Math.Pow(Math.Sin((lat2 - lat1) / 2), 2);
            var sdLon = Math.Pow(Math.Sin((lon2 - lon1) / 2), 2);
            var q = sdLat * sdLat + Math.Cos(lat1) * Math.Cos(lat2) * sdLon * sdLon;
            var d = 2 * r * Math.Asin(Math.Sqrt(q));

            return (int)d;
        }
    }
}
