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
    public class MapPoisPartDisplay : ContentPartDisplayDriver<MapPoisPart>
    {
        #region Dependencies

        private readonly IContentManager _contentManager;
        private readonly IMediaFileStore _mediaFileStore;

        #endregion

        #region Constructor

        public MapPoisPartDisplay(IContentManager contentManager, IMediaFileStore mediaFileStore)
        {
            _contentManager = contentManager;
            _mediaFileStore = mediaFileStore;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> EditAsync(MapPoisPart part, BuildPartEditorContext context)
        {
            var tiles = await GetTilesAsync(part);

            return Initialize<MapPoisEditViewModel>(GetEditorShapeType(context), model =>
            {
                model.ContentItem = part.ContentItem;

                if (tiles != null)
                {
                    model.Height = tiles.As<MapTiles>().Height;
                    model.Width = tiles.As<MapTiles>().Width;

                    // needs trailing slash otherwise doesn't load tile images
                    model.TileRoot = _mediaFileStore.MapPathToPublicUrl(GetTileRoot(tiles)) + "/";
                }
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(MapPoisPart part, IUpdateModel updater, UpdatePartEditorContext context)
        {
            var viewModel = new MapEditViewModel();

            await updater.TryUpdateModelAsync(viewModel, Prefix);

            return Edit(part, context);
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

        private async Task<ContentItem> GetTilesAsync(MapPoisPart map)
        {
            var field = map.ContentItem.Get<ContentPart>(Constants.MapContentType)?
                .Get<ContentPickerField>(Constants.MapTilesContentFieldName);

            if (field == null)
            {
                return null;
            }

            return await _contentManager.GetAsync(field.ContentItemIds.First());
        }

        #endregion
    }
}
