using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Controllers
{
    public class PoiController : Controller
    {
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IContentManager _contentManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;

        public PoiController(
            IContentItemDisplayManager contentItemDisplayManager,
            IContentManager contentManager,
            IUpdateModelAccessor updateModelAccessor)
        {
            _contentItemDisplayManager = contentItemDisplayManager;
            _contentManager = contentManager;
            _updateModelAccessor = updateModelAccessor;
        }

        public async Task<IActionResult> BuildDisplay(string contentItemId, string poiContentItemId)
        {
            if (string.IsNullOrWhiteSpace(contentItemId) || string.IsNullOrWhiteSpace(poiContentItemId))
            {
                return NotFound();
            }

            var contentItem = await _contentManager.GetAsync(contentItemId);

            if (contentItem == null || !contentItem.Has<MapPoisPart>())
            {
                return NotFound();
            }

            var part = contentItem.As<MapPoisPart>();
            var poiContentItem = part.ContentItems.SingleOrDefault(x => x.ContentItemId == poiContentItemId);

            if (poiContentItem == null)
            {
                return NotFound();
            }

            return View("Display", new BuildDisplayViewModel
            {
                DisplayShape = await _contentItemDisplayManager.BuildDisplayAsync(poiContentItem, _updateModelAccessor.ModelUpdater)
            });
        }
    }
}
