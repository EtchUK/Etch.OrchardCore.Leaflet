using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;

namespace Etch.OrchardCore.Leaflet
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition(Constants.TilesContentType, part => part
                .WithDescription("Converts image to tiles that can be used by leaflet map.")
                .WithDisplayName(Constants.TilesContentTypeDisplayName));

            _contentDefinitionManager.AlterPartDefinition(Constants.TilesContentType, part => part
                .WithField("MapImage", field => field
                    .OfType(typeof(MediaField).Name)
                    .WithDisplayName("Map Image")
                    .WithSettings(new MediaFieldSettings
                    {
                        Hint = "Recommend using a JPG image due to smaller file size.",
                        Multiple = false,
                        Required = true
                    })
                )
            );

            _contentDefinitionManager.AlterTypeDefinition(Constants.TilesContentType, type => type
                .Draftable()
                .Versionable()
                .Listable()
                .Creatable()
                .Securable()
                .WithPart("TitlePart")
                .WithPart(Constants.TilesContentType)
                .DisplayedAs(Constants.TilesContentTypeDisplayName));

            return 1;
        }
    }
}