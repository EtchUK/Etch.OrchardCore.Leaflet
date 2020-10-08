using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Drivers
{
    public class MapTilesDisplay : ContentPartDisplayDriver<MapTiles>
    {
        public override IDisplayResult Edit(MapTiles part, BuildPartEditorContext context)
        {
            return Initialize<MapTilesViewModel>(GetEditorShapeType(context), model =>
            {
                model.ContentItem = part.ContentItem;
                model.HasBeenProcessed = part.HasBeenProcessed;
                model.IsProcessing = part.IsProcessing;
                model.PublishAfterProcessed = part.ContentItem.CreatedUtc.HasValue ? part.PublishAfterProcessed : true;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(MapTiles model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            var viewModel = new MapTilesViewModel();

            await updater.TryUpdateModelAsync(viewModel, Prefix, t => t.PublishAfterProcessed);

            model.PublishAfterProcessed = viewModel.PublishAfterProcessed;

            return Edit(model, context);
        }
    }
}
