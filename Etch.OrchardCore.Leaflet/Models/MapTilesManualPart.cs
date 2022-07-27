using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Leaflet.Models
{
    public class MapTilesManualPart : ContentPart
    {
        public string TilesRoot { get; set; }

        public bool HasTileRoot
        {
            get { return !string.IsNullOrWhiteSpace(TilesRoot); }
        }
    }
}
