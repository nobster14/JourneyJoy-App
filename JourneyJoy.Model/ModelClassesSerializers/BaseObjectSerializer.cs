using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyJoy.Model.ModelClassesSerializers
{
    public static class BaseObjectSerializer<T>
    {
        public static string Serialize(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static T Deserialize(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
