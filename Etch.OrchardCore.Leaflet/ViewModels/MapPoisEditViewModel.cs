using Etch.OrchardCore.Leaflet.Dtos;
using Etch.OrchardCore.Leaflet.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using System;
using System.Collections.Generic;

namespace Etch.OrchardCore.Leaflet.ViewModels
{
    public class MapPoisEditViewModel
    {
        public ContentItem ContentItem { get; set; }
        public int Height { get; set; }
        public IEnumerable<PoiMarker> Markers { get; set; }
        public IList<ContentItem> Pois { get; set; }
        public string TileRoot { get; set; }
        public int Width { get; set; }

        public string[] ContentTypes { get; set; } = Array.Empty<string>();
        public string[] Prefixes { get; set; } = Array.Empty<string>();

        public bool HasTiles
        {
            get { return !string.IsNullOrWhiteSpace(TileRoot); }
        }

        [BindNever]
        public MapPoisPart MapPoisPart { get; set; }


        [BindNever]
        public IUpdateModel Updater { get; set; }
    }
}
