using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Leaflet.Models
{
    public class MapTiles : ContentPart
    {
        public bool HasBeenProcessed { get; set; }
        public bool IsProcessing { get; set; }
        public bool PublishAfterProcessed { get; set; }
    }
}
