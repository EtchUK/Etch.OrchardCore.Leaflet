using Etch.OrchardCore.Leaflet.Dtos;
using Etch.OrchardCore.Leaflet.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using System.Linq;

namespace Etch.OrchardCore.Leaflet.Extensions
{
    public static class PoiPartExtensions
    {
        public static PoiMarker GetMarker(this PoiPart part, IContentDefinitionManager contentDefinitionManager, bool isAdmin = false)
        {
            var poiPartSettings = contentDefinitionManager.GetTypeDefinition(part.ContentItem.ContentType)
                .Parts
                .SingleOrDefault(x => x.Name == nameof(PoiPart))
                .GetSettings<PoiPartSettings>();

            return new PoiMarker
            {
                AlwaysDisplay = part.ContentItem.As<PoiPart>().AlwaysDisplay,
                ContentItemId = part.ContentItem.ContentItemId,
                Icon = new PoiIcon
                {
                    Height = poiPartSettings.MarkerIconHeight,
                    HoverPath = poiPartSettings.MarkerHoverIcon,
                    Path = isAdmin ? poiPartSettings.AdminMarkerIconPath : poiPartSettings.MarkerIcon,
                    Width = poiPartSettings.MarkerIconWidth,
                    ZoomRatio = poiPartSettings.MarkerIconZoomRatio
                },
                Latitude = part.Latitude,
                Longitude = part.Longitude,
                Title = part.ContentItem.DisplayText,
                ZoomLevels = part.ContentItem.As<PoiPart>().ZoomLevels
            };
        }
    }
}
