using Etch.OrchardCore.Leaflet.Indexes;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
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
                .WithField(Constants.TilesMediaFileFieldName, field => field
                    .OfType(nameof(MediaField))
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

            SchemaBuilder.CreateMapIndexTable(nameof(MapTilesIndex), table => table
                .Column<bool>(nameof(MapTilesIndex.HasBeenProcessed))
            );

            SchemaBuilder.AlterTable(nameof(MapTilesIndex), table => table
                .CreateIndex(
                    $"IDX_{nameof(MapTilesIndex)}_{nameof(MapTilesIndex.HasBeenProcessed)}",
                    nameof(MapTilesIndex.HasBeenProcessed))
            );

            return 1;
        }

        public int UpdateFrom1()
        {
            _contentDefinitionManager.AlterPartDefinition(Constants.MapContentType, part => part
                .WithDescription("Displays map.")
                .WithDisplayName(Constants.TilesContentTypeDisplayName));

            _contentDefinitionManager.AlterPartDefinition(Constants.MapContentType, part => part
                .WithField(Constants.MapTilesContentFieldName, field => field
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Tiles")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        DisplayedContentTypes = new string[] { Constants.TilesContentType },
                        Multiple = false,
                        Required = true
                    })
                )
            );

            _contentDefinitionManager.AlterTypeDefinition(Constants.MapContentType, type => type
                .Draftable()
                .Versionable()
                .Listable()
                .Creatable()
                .Securable()
                .WithPart("TitlePart")
                .WithPart("AutoroutePart", part => part.WithSettings(new AutoroutePartSettings
                {
                    AllowCustomPath = true,
                    Pattern = "{{ Model.ContentItem | display_text | slugify }}",
                    ShowHomepageOption = true
                }))
                .WithPart(Constants.MapContentType)
                .DisplayedAs(Constants.MapContentType));

            return 2;
        }

        public int UpdateFrom2()
        {
            _contentDefinitionManager.AlterPartDefinition(Constants.MapContentType, part => part
                .WithField(Constants.MapInitialZoomFieldName, field => field
                    .OfType(nameof(NumericField))
                    .WithDisplayName("Initial Zoom")
                    .WithSettings(new NumericFieldSettings
                    {
                        DefaultValue = Constants.DefaultInitialZoom.ToString(),
                        Maximum = Constants.MaxZoomLevel,
                        Minimum = 0,
                        Required = true,
                        Scale = 1
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.MapContentType, part => part
                .WithField(Constants.MapMaxZoomFieldName, field => field
                    .OfType(nameof(NumericField))
                    .WithDisplayName("Maximum Zoom")
                    .WithSettings(new NumericFieldSettings
                    {
                        DefaultValue = Constants.DefaultMaxZoom.ToString(),
                        Maximum = Constants.MaxZoomLevel,
                        Minimum = 0,
                        Required = true,
                        Scale = 1
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(Constants.MapContentType, part => part
                .WithField(Constants.MapMinZoomFieldName, field => field
                    .OfType(nameof(NumericField))
                    .WithDisplayName("Minimum Zoom")
                    .WithSettings(new NumericFieldSettings
                    {
                        DefaultValue = Constants.DefaultMinZoom.ToString(),
                        Maximum = Constants.MaxZoomLevel,
                        Minimum = 0,
                        Required = true,
                        Scale = 1
                    })
                )
            );

            return 3;
        }
    }
}