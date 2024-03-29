﻿namespace Etch.OrchardCore.Leaflet
{
    public static class Constants
    {
        public const int DefaultInitialZoom = 11;
        public const int DefaultMaxZoom = 14;
        public const int DefaultMinZoom = 8;
        public const bool DefaultMouseWheelZoom = false;
        public const string DefaultZoomControlPosition = "topleft";
        public const int MaxZoomLevel = 15;

        public const string MapContentType = "Map";
        public const string MapTilesContentFieldName = "Tiles";
        public const string MapInitialZoomFieldName = "InitialZoom";
        public const string MapMaxZoomFieldName = "MaxZoom";
        public const string MapMinZoomFieldName = "MinZoom";
        public const string MapMouseWheelZoom = "MouseWheelZoom";
        public const string MapZoomControlPositionFieldName = "ZoomControlPosition";

        public const string PoiAlwaysDisplayField = "AlwaysDisplay";
        public const string PoiZoomLevelsField = "ZoomLevels";

        public const string PoiSelectEventCategoryFieldName = "PoiSelectEventCategory";
        public const string PoiSelectEventActionFieldName = "PoiSelectEventAction";

        public const string TilesContentType = "MapTiles";
        public const string TilesContentTypeDisplayName = "Map Tiles";
        public const string TilesMediaFileFieldName = "MapImage";
    }
}
