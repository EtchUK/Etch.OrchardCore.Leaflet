using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Drivers
{
    public class PoiPartDisplay : ContentPartDisplayDriver<PoiPart>
    {
        #region Dependencies

        #endregion

        #region Constructor

        public PoiPartDisplay()
        {
            
        }

        #endregion

        #region Overrides

        public override IDisplayResult Edit(PoiPart part, BuildPartEditorContext context)
        {
            return Initialize<PoiPartEditViewModel>(GetEditorShapeType(context), model =>
            {
                model.Latitude = part.Latitude;
                model.Longitude = part.Longitude;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(PoiPart model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            var viewModel = new PoiPartEditViewModel();

            await updater.TryUpdateModelAsync(viewModel, Prefix, t => t.Latitude, t => t.Longitude);

            model.Latitude = viewModel.Latitude;
            model.Longitude = viewModel.Longitude;

            return Edit(model, context);
        }

        #endregion
    }
}
