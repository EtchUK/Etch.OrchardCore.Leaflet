using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Settings
{
    public class PoiPartSettingsDisplayDriver : ContentTypePartDefinitionDisplayDriver
    {
        public override IDisplayResult Edit(ContentTypePartDefinition model, IUpdateModel updater)
        {
            if (!string.Equals(nameof(PoiPart), model.PartDefinition.Name))
            {
                return null;
            }

            return Initialize<PoiPartSettingsViewModel>("PoiPartSettings_Edit", viewModel =>
            {
                var settings = model.GetSettings<PoiPartSettings>();

                viewModel.MarkerIcon = settings.MarkerIcon;
                viewModel.MarkerIconHeight = settings.MarkerIconHeight;
                viewModel.MarkerIconWidth = settings.MarkerIconWidth;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTypePartDefinition model, UpdateTypePartEditorContext context)
        {
            if (!string.Equals(nameof(PoiPart), model.PartDefinition.Name))
            {
                return null;
            }

            var viewModel = new PoiPartSettingsViewModel();

            await context.Updater.TryUpdateModelAsync(viewModel, Prefix, m => m.MarkerIcon);

            context.Builder.WithSettings(new PoiPartSettings
            {
                MarkerIcon = viewModel.MarkerIcon,
                MarkerIconHeight = viewModel.MarkerIconHeight,
                MarkerIconWidth = viewModel.MarkerIconWidth
            });

            return Edit(model, context.Updater);
        }
    }
}
