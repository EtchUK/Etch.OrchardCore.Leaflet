using Etch.OrchardCore.Leaflet.Extensions;
using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Media;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Drivers
{
    public class MapDisplay : ContentPartDisplayDriver<Map>
    {
        #region Dependencies

        private readonly IContentManager _contentManager;
        private readonly IMediaFileStore _mediaFileStore;

        #endregion

        #region Constructor

        public MapDisplay(IContentManager contentManager, IMediaFileStore mediaFileStore)
        {
            _contentManager = contentManager;
            _mediaFileStore = mediaFileStore;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> DisplayAsync(Map map, BuildPartDisplayContext context)
        {
            var tiles = await GetTilesAsync(map);

            return Initialize<MapViewModel>("Map", model =>
            {
                model.ContentItem = map.ContentItem;
                model.Height = tiles.As<MapTiles>().Height;
                model.InitialZoom = map.InitialZoom;
                model.MaxZoom = map.MaxZoom;
                model.MinZoom = map.MinZoom;
                model.Width = tiles.As<MapTiles>().Width;

                // needs trailing slash otherwise doesn't load tile images
                model.TileRoot = _mediaFileStore.MapPathToPublicUrl(GetTileRoot(tiles)) + "/";
            })
            .Location("Detail", "Content:5");
        }

        #endregion

        #region HelperMethods

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
            return await _contentManager.GetAsync(field.ContentItemIds.First());
        }

        #endregion
    }
}