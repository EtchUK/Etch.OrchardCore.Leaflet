using Etch.OrchardCore.Leaflet.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Etch.OrchardCore.Leaflet.Utils
{
    public static class PoiUtils
    {
        public static string Serialize(IEnumerable<PoiMarker> markers)
        {
            return JsonConvert.SerializeObject(markers, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }).Replace("'", @"\'");
        }
    }
}
