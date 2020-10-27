using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
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
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IShapeFactory _shapeFactory;
        private readonly IUpdateModelAccessor _updateModelAccessor;

        public AdminController(
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            IContentItemDisplayManager contentItemDisplayManager,
            IShapeFactory shapeFactory,
            IUpdateModelAccessor updateModelAccessor)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _shapeFactory = shapeFactory;
            _updateModelAccessor = updateModelAccessor;
        }

        public async Task<IActionResult> BuildEditor(string id, string prefix, string prefixesName, string contentTypesName, string targetId, bool flowmetadata, string parentContentType, string partName)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var contentItem = await _contentManager.NewAsync(id);
            var cardCollectionType = nameof(MapPoisPart);

            //Create a Card Shape
            dynamic contentCard = await _shapeFactory.New.ContentCard(
                //Updater is the controller for AJAX Requests
                Updater: _updateModelAccessor.ModelUpdater,
                //Shape Specific
                CollectionShapeType: cardCollectionType,
                ContentItem: contentItem,
                BuildEditor: true,
                ParentContentType: parentContentType,
                CollectionPartName: partName,
                ContainedContentTypes: _contentDefinitionManager.ListTypeDefinitions().Where(t => t.GetSettings<ContentTypeSettings>().Stereotype == "POI"),
                //Card Specific Properties
                TargetId: targetId,
                Inline: false,
                CanMove: false,
                CanDelete: true,
                //Input hidden
                //Prefixes
                HtmlFieldPrefix: prefix,
                PrefixesId: prefixesName.Replace('.', '_'),
                PrefixesName: prefixesName,
                //ContentTypes
                ContentTypesId: contentTypesName.Replace('.', '_'),
                ContentTypesName: contentTypesName
            );

            var model = new BuildEditorViewModel
            {
                EditorShape = contentCard
            };

            return View("Display", model);
        }
    }
}
