namespace Etch.OrchardCore.Leaflet.Models
{
    public class PoiPartSettings
    {
        public string MarkerAdminIcon { get; set; }
        public string MarkerIcon { get; set; }
        public int MarkerIconHeight { get; set; }
        public int MarkerIconWidth { get; set; }

        public string AdminMarkerIconPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MarkerAdminIcon))
                {
                    return MarkerIcon;
                }

                return MarkerAdminIcon;
            }
        }
    }
}
