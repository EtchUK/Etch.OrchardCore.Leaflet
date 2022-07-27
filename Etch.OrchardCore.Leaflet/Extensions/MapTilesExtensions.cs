using Etch.OrchardCore.Leaflet.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Media.Fields;
using System.IO;
using System.Linq;

namespace Etch.OrchardCore.Leaflet.Extensions
{
    public static class MapTilesExtensions
    {
        public static string GetTileRoot(this MapTiles part)
        {
            if (part.ContentItem.Has<MapTilesManualPart>())
            {
                return part.ContentItem.As<MapTilesManualPart>().TilesRoot;
            }

            var mediaField = part.Get<MediaField>(Constants.TilesMediaFileFieldName);

            if (mediaField == null || mediaField.Paths == null || !mediaField.Paths.Any())
            {
                return null;
            }

            return Path.Combine(Path.GetDirectoryName(mediaField.Paths[0]), part.ContentItem.ContentItemId);
        }
    }
}
