﻿using Etch.OrchardCore.Fields.Values.Fields;
using Etch.OrchardCore.Leaflet.Indexes;
using Etch.OrchardCore.Leaflet.Models;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using YesSql;
using YesSql.Sql;

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

            SchemaBuilder.CreateMapIndexTable<MapTilesIndex>(table => table
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

        public int UpdateFrom3()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(PoiPart), builder => builder
                .Attachable()
                .WithDescription("Provides configuration for POI markers."));

            return 4;
        }

        public int UpdateFrom4()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(MapPoisPart), builder => builder
                .WithDescription("Collection of POIs for a map.")
                .WithDisplayName("Map POIs"));

            _contentDefinitionManager.AlterTypeDefinition(Constants.MapContentType, type => type
                .WithPart(nameof(MapPoisPart)));

            return 5;
        }

        public int UpdateFrom5()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(MapPoisPart), builder => builder
                .WithField(Constants.PoiSelectEventCategoryFieldName, field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName("POI Select Event Category")
                )
                .WithField(Constants.PoiSelectEventActionFieldName, field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName("POI Select Event Action")
                )
            );

            return 6;
        }

        public int UpdateFrom6()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(MapTilesManualPart), builder => builder
                .WithDescription("Allow users to specify the directory where map tiles are located.")
                .WithDisplayName("Map Tiles Manual")
                .Attachable());

            return 7;
        }

        public int UpdateFrom7()
        {
            _contentDefinitionManager.AlterPartDefinition(Constants.MapContentType, part => part
                .WithField(Constants.MapZoomControlPositionFieldName, field => field
                    .OfType(nameof(TextField))
                    .WithDisplayName("Zoom Control Position")
                    .WithEditor("PredefinedList")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Determine the position of the zoom control."
                    })
                    .WithSettings(new TextFieldPredefinedListEditorSettings
                    {
                        DefaultValue = Constants.DefaultZoomControlPosition,
                        Editor = EditorOption.Dropdown,
                        Options = new[]
                        {
                            new ListValueOption
                            {
                                Name = "Top Left",
                                Value = "topleft",
                            },
                            new ListValueOption
                            {
                                Name = "Top Right",
                                Value = "topright",
                            },
                            new ListValueOption
                            {
                                Name = "Bottom Left",
                                Value = "bottomleft",
                            },
                            new ListValueOption
                            {
                                Name = "Bottom Right",
                                Value = "bottomright",
                            }
                        },
                    })
                )
            );

            return 8;
        }

        public int UpdateFrom8()
        {
            _contentDefinitionManager.AlterPartDefinition(Constants.MapContentType, part => part
                .WithField(Constants.MapMouseWheelZoom, field => field
                    .OfType(nameof(BooleanField))
                    .WithDisplayName("Mouse Wheel Scroll")
                    .WithSettings(new BooleanFieldSettings
                    {
                        Hint = "Allow users to use the mouse wheel to zoom in & out of map.",
                        Label = "Enable Mouse Wheel Zoom"
                    })
                )
            );

            return 9;
        }

        public int UpdateFrom9()
        {
            _contentDefinitionManager.AlterPartDefinition(nameof(PoiPart), part => part
                .WithField(Constants.PoiAlwaysDisplayField, field => field
                    .OfType(nameof(BooleanField))
                    .WithDisplayName("Always Display")
                    .WithPosition("0")
                    .WithSettings(new BooleanFieldSettings
                    {
                        DefaultValue = true,
                        Hint = "Display POI on all zoom levels.",
                        Label = "Always Display"
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition(nameof(PoiPart), part => part
                .WithField(Constants.PoiZoomLevelsField, field => field
                    .OfType(nameof(ValuesField))
                    .WithDisplayName("Zoom Levels")
                    .WithPosition("1")
                    .WithSettings(new Fields.Values.Settings.ValuesFieldSettings
                    {
                        Hint = "Enter the zoom level(s) (e.g. 12) that POI should be visible on."
                    })
                )
            );

            return 10;
        }
    }
}