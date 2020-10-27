﻿using Etch.OrchardCore.Leaflet.Dtos;
using Etch.OrchardCore.Leaflet.Models;
using OrchardCore.ContentManagement.Metadata;
using System.Linq;

namespace Etch.OrchardCore.Leaflet.Extensions
{
    public static class PoiPartExtensions
    {
        public static PoiMarker GetMarker(this PoiPart part, IContentDefinitionManager contentDefinitionManager)
        {
            var poiPartSettings = contentDefinitionManager.GetTypeDefinition(part.ContentItem.ContentType)
                .Parts
                .SingleOrDefault(x => x.Name == nameof(PoiPart))
                .GetSettings<PoiPartSettings>();

            return new PoiMarker
            {
                ContentItemId = part.ContentItem.ContentItemId,
                Icon = new PoiIcon
                {
                    Height = poiPartSettings.MarkerIconHeight,
                    Path = poiPartSettings.MarkerIcon,
                    Width = poiPartSettings.MarkerIconWidth
                },
                Latitude = part.Latitude,
                Longitude = part.Longitude
            };
        }
    }
}