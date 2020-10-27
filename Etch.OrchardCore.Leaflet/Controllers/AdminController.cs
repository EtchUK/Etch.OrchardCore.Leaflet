using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Controllers
{
    public class AdminController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IShapeFactory _shapeFactory;
        private readonly IUpdateModelAccessor _updateModelAccessor;

        public AdminController(
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            IShapeFactory shapeFactory,
            IUpdateModelAccessor updateModelAccessor)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _shapeFactory = shapeFactory;
            _updateModelAccessor = updateModelAccessor;
        }

        public async Task<IActionResult> BuildEditor(BuildEditorViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                return NotFound();
            }

            var contentItem = await _contentManager.NewAsync(model.Id);
            var cardCollectionType = nameof(MapPoisPart);

            model.EditorShape = await _shapeFactory.New.ContentCard(
                //Updater is the controller for AJAX Requests
                Updater: _updateModelAccessor.ModelUpdater,
                //Shape Specific
                CollectionShapeType: cardCollectionType,
                ContentItem: contentItem,
                BuildEditor: true,
                ParentContentType: model.ParentContentType,
                CollectionPartName: model.PartName,
                ContainedContentTypes: _contentDefinitionManager.ListTypeDefinitions().Where(t => t.GetSettings<ContentTypeSettings>().Stereotype == "POI"),
                //Card Specific Properties
                TargetId: model.TargetId,
                Inline: false,
                CanMove: false,
                CanDelete: true,
                //Input hidden
                //Prefixes
                HtmlFieldPrefix: model.Prefix,
                PrefixesId: model.PrefixesName.Replace('.', '_'),
                PrefixesName: model.PrefixesName,
                //ContentTypes
                ContentTypesId: model.ContentTypesName.Replace('.', '_'),
                ContentTypesName: model.ContentTypesName
            );

            return View("Display", model);
        }
    }
}
