using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace Etch.OrchardCore.Leaflet.Models
{
    public class MapPoisPart : ContentPart
    {
        [BindNever]
        public List<ContentItem> ContentItems { get; set; } = new List<ContentItem>();
    }
}
