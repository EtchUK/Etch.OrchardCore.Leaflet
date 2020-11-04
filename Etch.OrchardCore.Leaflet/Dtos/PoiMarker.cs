using Newtonsoft.Json;

namespace Etch.OrchardCore.Leaflet.Dtos
{
    public class PoiMarker
    {
        public string ContentItemId { get; set; }
        public PoiIcon Icon { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
        public string Title { get; set; }
    }
}
