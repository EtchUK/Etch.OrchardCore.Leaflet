using Newtonsoft.Json;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Leaflet.Models
{
    public class Map : ContentPart
    {
        [JsonIgnore]
        public decimal InitialZoom
        {
            get { return this.Get<NumericField>(Constants.MapInitialZoomFieldName)?.Value ?? Constants.DefaultInitialZoom; }
        }

        [JsonIgnore]
        public decimal MaxZoom
        {
            get { return this.Get<NumericField>(Constants.MapMaxZoomFieldName)?.Value ?? Constants.DefaultMaxZoom; }
        }

        [JsonIgnore]
        public decimal MinZoom
        {
            get { return this.Get<NumericField>(Constants.MapMinZoomFieldName)?.Value ?? Constants.DefaultMinZoom; }
        }

        [JsonIgnore]
        public bool MouseWheelZoom
        {
            get { return this.Get<BooleanField>(Constants.MapMouseWheelZoom)?.Value ?? Constants.DefaultMouseWheelZoom; }
        }

        [JsonIgnore]
        public string ZoomControlPosition
        {
            get { return this.Get<TextField>(Constants.MapZoomControlPositionFieldName)?.Text ?? Constants.DefaultZoomControlPosition; }
        }
    }
}
