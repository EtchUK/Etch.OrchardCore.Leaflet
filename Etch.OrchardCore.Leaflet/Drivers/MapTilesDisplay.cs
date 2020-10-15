using Etch.OrchardCore.Leaflet.Extensions;
using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Media;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Drivers
{
    public class MapTilesDisplay : ContentPartDisplayDriver<MapTiles>
    {
        #region Dependencies

        private readonly IMediaFileStore _mediaFileStore;

        #endregion

        #region Constructor

        public MapTilesDisplay(IMediaFileStore mediaFileStore)
        {
            _mediaFileStore = mediaFileStore;
        }

        #endregion

        #region Overrides

        public override IDisplayResult Edit(MapTiles part, BuildPartEditorContext context)
        {
            return Initialize<MapTilesViewModel>(GetEditorShapeType(context), model =>
            {
                model.ContentItem = part.ContentItem;
                model.HasBeenProcessed = part.HasBeenProcessed;
                model.Height = part.Height;
                model.IsProcessing = part.IsProcessing;
                model.PublishAfterProcessed = !part.ContentItem.CreatedUtc.HasValue || part.PublishAfterProcessed;
                model.Width = part.Width;

                // needs trailing slash otherwise doesn't load tile images
                model.TileRoot = _mediaFileStore.MapPathToPublicUrl(part.GetTileRoot()) + "/";
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(MapTiles model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            var viewModel = new MapTilesViewModel();

            await updater.TryUpdateModelAsync(viewModel, Prefix, t => t.Height, t => t.PublishAfterProcessed, t => t.Width);

            model.Height = viewModel.Height;
            model.PublishAfterProcessed = viewModel.PublishAfterProcessed;
            model.Width = viewModel.Width;

            return Edit(model, context);
        }

        #endregion
    }
}
