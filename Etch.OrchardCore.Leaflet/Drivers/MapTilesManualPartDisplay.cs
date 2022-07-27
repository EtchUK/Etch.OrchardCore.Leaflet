using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Drivers
{
    public class MapTilesManualPartDisplay : ContentPartDisplayDriver<MapTilesManualPart>
    {
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

            part.TilesRoot = model.TilesRoot;

            return Edit(part);
        }
    }
}
