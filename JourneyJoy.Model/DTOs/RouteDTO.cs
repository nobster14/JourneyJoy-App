using JourneyJoy.Model.Database.Tables;
using JourneyJoy.Model.ModelClassesSerializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.DTOs
{
    public record RouteDTO
    {
        public Guid Id { get; set; }
        public int StartDay { get; set; }
        public Guid[][] AttractionsInOrder { get; set; }

        public Guid StarePointAttractionId { get; set; }

        public static RouteDTO FromDatabaseRoute(Route route)
        {
            return new RouteDTO()
            {
                StartDay = route.StartDay,
                Id = route.Id,
                AttractionsInOrder = BaseObjectSerializer<Guid[][]>.Deserialize(route.SerializedAttractionsIds),
                StarePointAttractionId = route.StartPointAttractionId
            };
        }

        public static Guid[][] CreateAttractionsInOrder(List<AttractionDTO> attractions, List<int>[] returnedRoute)
        {
            var ret = new Guid[returnedRoute.Length][];

            foreach (var i in Enumerable.Range(0, returnedRoute.Length))     
                ret[i] = returnedRoute[i].Select(it => attractions[it].Id).ToArray();
            
            return ret;
        }
    }


}
