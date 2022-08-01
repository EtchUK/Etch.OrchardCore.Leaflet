using Etch.OrchardCore.Leaflet.Dtos;
using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace Etch.OrchardCore.Leaflet.ViewModels
{
    public class MapViewModel
    {
        public MapAnalyticsViewModel Analytics { get; set; }
        public ContentItem ContentItem { get; set; }
        public int Height { get; set; }
        public decimal InitialZoom { get; set; }
        public IEnumerable<PoiMarker> Markers { get; set; }
        public decimal MaxZoom { get; set; }
        public decimal MinZoom { get; set; }
        public string TileRoot { get; set; }
        public int Width { get; set; }
        public string ZoomControlPosition { get; set; }

        public double Ratio
        {
            get
            {
                return ((double)Height / (double)Width) * 100;
            }
        }
    }
}
