using Etch.OrchardCore.Leaflet.Dtos;
using Etch.OrchardCore.Leaflet.Extensions;
using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Media;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Drivers
{
    public class MapDisplay : ContentPartDisplayDriver<Map>
    {
        #region Dependencies

        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IContentManager _contentManager;
        private readonly IMediaFileStore _mediaFileStore;

        #endregion

        #region Constructor

        public MapDisplay(IContentDefinitionManager contentDefinitionManager, IContentManager contentManager, IMediaFileStore mediaFileStore)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            _mediaFileStore = mediaFileStore;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> DisplayAsync(Map part, BuildPartDisplayContext context)
        {
            var tiles = await GetTilesAsync(part);

            return Initialize<MapViewModel>("Map", model =>
            {
                model.Analytics = GetAnalytics(part.ContentItem);
                model.ContentItem = part.ContentItem;
                model.Height = tiles.As<MapTiles>().Height;
                model.InitialZoom = part.InitialZoom;
                model.Markers = GetMarkers(part.ContentItem);
                model.MaxZoom = part.MaxZoom;
                model.MinZoom = part.MinZoom;
                model.Width = tiles.As<MapTiles>().Width;

                // needs trailing slash otherwise doesn't load tile images
                model.TileRoot = _mediaFileStore.MapPathToPublicUrl(GetTileRoot(tiles)) + "/";
            })
            .Location("Detail", "Content:5");
        }

        #endregion

        #region HelperMethods

        private MapAnalyticsViewModel GetAnalytics(ContentItem contentItem)
        {
            var part = contentItem.As<MapPoisPart>();

            if (part == null)
            {
                return null;
            }

            return new MapAnalyticsViewModel
            {
                PoiSelectEventAction = part.Get<TextField>(Constants.PoiSelectEventActionFieldName)?.Text,
                PoiSelectEventCategory = part.Get<TextField>(Constants.PoiSelectEventCategoryFieldName)?.Text,
            };
        }

        private IEnumerable<PoiMarker> GetMarkers(ContentItem contentItem)
        {
            var part = contentItem.As<MapPoisPart>();

            if (part == null)
            {
                return new List<PoiMarker>();
            }

            return part.ContentItems
                .Where(x => x.As<PoiPart>() != null)
                .Select(x => x.As<PoiPart>().GetMarker(_contentDefinitionManager));
        }

        private string GetTileRoot(ContentItem contentItem)
        {
            if (contentItem == null)
            {
                return string.Empty;
            }

            return contentItem.As<MapTiles>().GetTileRoot();
        }

        private async Task<ContentItem> GetTilesAsync(Map map)
        {
            var field = map.Get<ContentPickerField>(Constants.MapTilesContentFieldName);

            if (field == null)
            {
                return null;
            }

            return await _contentManager.GetAsync(field.ContentItemIds.First());
        }

        #endregion
    }
}