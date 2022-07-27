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
    public class MapTilesManualPartDisplay : ContentPartDisplayDriver<MapTilesManualPart>
    {
        private readonly IMediaFileStore _mediaFileStore;

        public MapTilesManualPartDisplay(IMediaFileStore mediaFileStore)
        {
            _mediaFileStore = mediaFileStore;
        }

        public override IDisplayResult Edit(MapTilesManualPart part, BuildPartEditorContext context)
        {
            return Initialize<MapTilesManualPartEditViewModel>(GetEditorShapeType(context), model =>
            {
                model.TilesRoot = part.TilesRoot;
            }).Location("Content:5");
        }

        public override async Task<IDisplayResult> UpdateAsync(MapTilesManualPart part, IUpdateModel updater, UpdatePartEditorContext context)
        {
            var model = new MapTilesManualPartEditViewModel();

            if (!await updater.TryUpdateModelAsync(model, Prefix))
            {
                return Edit(part);
            }

            if (await _mediaFileStore.GetDirectoryInfoAsync(model.TilesRoot) == null)
            {
                updater.ModelState.AddModelError(nameof(MapTilesManualPart.TilesRoot), "Directory doesn't exist in media library.");
                return Edit(part, context);
            }

            part.TilesRoot = model.TilesRoot;

            return Edit(part);
        }
    }
}
