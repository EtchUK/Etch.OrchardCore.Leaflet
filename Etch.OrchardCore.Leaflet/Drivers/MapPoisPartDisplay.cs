﻿using Etch.OrchardCore.Leaflet.Extensions;
using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Leaflet.Drivers
{
    public class MapPoisPartDisplay : ContentPartDisplayDriver<MapPoisPart>
    {
        #region Dependencies

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IMediaFileStore _mediaFileStore;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Constructor

        public MapPoisPartDisplay(IContentManager contentManager, IContentDefinitionManager contentDefinitionManager, IMediaFileStore mediaFileStore, IServiceProvider serviceProvider)
        {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _mediaFileStore = mediaFileStore;
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Overrides

        public override async Task<IDisplayResult> EditAsync(MapPoisPart part, BuildPartEditorContext context)
        {
            var tiles = await GetTilesAsync(part);

            return Initialize<MapPoisEditViewModel>(GetEditorShapeType(context), model =>
            {
                model.ContentItem = part.ContentItem;
                model.MapPoisPart = part;
                model.Markers = part.ContentItems.Where(x => x.As<PoiPart>() != null).Select(x => x.As<PoiPart>().GetMarker(_contentDefinitionManager, true));
                model.Updater = context.Updater;

                if (tiles != null)
                {
                    model.Height = tiles.As<MapTiles>().Height;
                    model.Width = tiles.As<MapTiles>().Width;

                    // needs trailing slash otherwise doesn't load tile images
                    model.TileRoot = _mediaFileStore.MapPathToPublicUrl(GetTileRoot(tiles)) + "/";
                }
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(MapPoisPart part, IUpdateModel updater, UpdatePartEditorContext context)
        {
            var contentItemDisplayManager = _serviceProvider.GetRequiredService<IContentItemDisplayManager>();
            var model = new MapPoisEditViewModel { MapPoisPart = part };

            await context.Updater.TryUpdateModelAsync(model, Prefix);

            var contentItems = new List<ContentItem>();

            for (var i = 0; i < model.Prefixes.Length; i++)
            {
                var contentItem = await _contentManager.NewAsync(model.ContentTypes[i]);
                var existingContentItem = part.ContentItems.FirstOrDefault(x => string.Equals(x.ContentItemId, model.Prefixes[i], StringComparison.OrdinalIgnoreCase));

                if (existingContentItem != null)
                {
                    contentItem.ContentItemId = model.Prefixes[i];
                    contentItem.Merge(existingContentItem);
                }

                await contentItemDisplayManager.UpdateEditorAsync(contentItem, context.Updater, context.IsNew, htmlFieldPrefix: model.Prefixes[i]);

                contentItems.Add(contentItem);
            }

            part.ContentItems = contentItems;

            return Edit(part, context);
        }

        #endregion

        #region HelperMethods

        private string GetTileRoot(ContentItem contentItem)
        {
            if (contentItem == null)
            {
                return string.Empty;
            }

            return contentItem.As<MapTiles>().GetTileRoot();
        }

        private async Task<ContentItem> GetTilesAsync(MapPoisPart map)
        {
            var field = map.ContentItem.Get<ContentPart>(Constants.MapContentType)?
                .Get<ContentPickerField>(Constants.MapTilesContentFieldName);

            if (field == null)
            {
                return null;
            }

            return await _contentManager.GetAsync(field.ContentItemIds.First());
        }

        #endregion
    }
}
