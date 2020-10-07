using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;

namespace Etch.OrchardCore.Leaflet.Drivers
{
    public class MapTilesDisplay : ContentPartDisplayDriver<MapTiles>
    {
        public override IDisplayResult Edit(MapTiles part, BuildPartEditorContext context)
        {
            return Initialize<MapTilesViewModel>(GetEditorShapeType(context), model =>
            {
                model.ContentItem = part.ContentItem;
            });
        }
    }
}
