using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace Etch.OrchardCore.Leaflet.ViewModels
{
    public class MapPoisEditViewModel
    {
        public ContentItem ContentItem { get; set; }
        public int Height { get; set; }
        public IList<ContentItem> Pois { get; set; }
        public string TileRoot { get; set; }
        public int Width { get; set; }

        public bool HasTiles
        {
            get { return !string.IsNullOrWhiteSpace(TileRoot); }
        }
    }
}
