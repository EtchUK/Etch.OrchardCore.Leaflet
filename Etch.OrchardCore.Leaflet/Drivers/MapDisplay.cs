using Etch.OrchardCore.Leaflet.Extensions;
using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
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
            var tileRoot = await GetTileRootAsync(map);

            return Initialize<MapViewModel>("Map", model =>
            {
                model.ContentItem = map.ContentItem;

                // needs trailing slash otherwise doesn't load tile images
                model.TileRoot = _mediaFileStore.MapPathToPublicUrl(tileRoot) + "/";
            })
            .Location("Detail", "Content:5");

        }

        #endregion

        #region HelperMethods

        private async Task<string> GetTileRootAsync(Map map)
        {
            var field = map.Get<ContentPickerField>(Constants.MapTilesContentFieldName);
            var contentItem = await _contentManager.GetAsync(field.ContentItemIds.First());

            if (contentItem == null)
            {
                return string.Empty;
            }

            return contentItem.As<MapTiles>().GetTileRoot();
        }

        #endregion
    }
}