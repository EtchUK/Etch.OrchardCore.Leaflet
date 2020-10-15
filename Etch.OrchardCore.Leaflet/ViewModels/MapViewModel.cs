using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Leaflet.ViewModels
{
    public class MapViewModel
    {
        public ContentItem ContentItem { get; set; }
        public int Height { get; set; }
        public string TileRoot { get; set; }
        public int Width { get; set; }

        public double Ratio
        {
            get
            {
                return ((double)Height / (double)Width) * 100;
            }
        }
    }
}
