﻿using OrchardCore.ContentManagement;

namespace Etch.OrchardCore.Leaflet.ViewModels
{
    public class MapTilesViewModel
    {
        public ContentItem ContentItem { get; set; }

        public bool HasBeenProcessed { get; set; }
        public int Height { get; set; }
        public bool IsProcessing { get; set; }
        public bool PublishAfterProcessed { get; set; }
        public string TileRoot { get; set; }
        public int Width { get; set; }
    }
}
