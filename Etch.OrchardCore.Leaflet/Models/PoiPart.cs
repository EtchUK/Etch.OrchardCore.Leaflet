using Etch.OrchardCore.Fields.Values.Fields;
using Newtonsoft.Json;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;

namespace Etch.OrchardCore.Leaflet.Models
{
    public class PoiPart : ContentPart
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [JsonIgnore]
        public bool AlwaysDisplay
        {
            get { return this.Get<BooleanField>(Constants.PoiAlwaysDisplayField)?.Value ?? true; }
        }

        [JsonIgnore]
        public int[] ZoomLevels
        {
            get
            {
                var values = new List<int>();

                foreach (var value in this.Get<ValuesField>(Constants.PoiZoomLevelsField)?.Data ?? Array.Empty<string>())
                {
                    if (int.TryParse(value, out int result))
                    {
                        values.Add(result);
                    }
                }

                return values.ToArray();
            }
        }
    }
}
