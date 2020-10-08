using Etch.OrchardCore.Leaflet.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace Etch.OrchardCore.Leaflet.Indexes
{
    public class MapTilesIndex : MapIndex
    {
        public bool HasBeenProcessed { get; set; }
    }

    public class MapTilesIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<MapTilesIndex>()
                .Map(contentItem =>
                {
                    var mapTiles = contentItem.As<MapTiles>();

                    if (mapTiles == null)
                    {
                        return null;
                    }

                    // Remove index for items that are not the latest version
                    if (!contentItem.Latest)
                    {
                        return null;
                    }

                    return new MapTilesIndex
                    {
                        HasBeenProcessed = mapTiles.HasBeenProcessed,
                    };
                });
        }
    }
}
