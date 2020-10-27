using Etch.OrchardCore.Leaflet.Controllers;
using Etch.OrchardCore.Leaflet.Drivers;
using Etch.OrchardCore.Leaflet.Indexes;
using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.Services;
using Etch.OrchardCore.Leaflet.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using System;
using YesSql.Indexes;

namespace Etch.OrchardCore.Leaflet
{
    public class Startup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public Startup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentPart<MapTiles>()
                .UseDisplayDriver<MapTilesDisplay>();

            services.AddContentPart<Map>()
                .UseDisplayDriver<MapDisplay>();

            services.AddContentPart<MapPoisPart>()
                .UseDisplayDriver<MapPoisPartDisplay>();

            services.AddContentPart<PoiPart>()
                .UseDisplayDriver<PoiPartDisplay>();

            services.AddScoped<IContentTypePartDefinitionDisplayDriver, PoiPartSettingsDisplayDriver>();

            services.AddScoped<IDataMigration, Migrations>();

            services.AddSingleton<IBackgroundTask, TileGeneratorBackgroundTask>();
            services.AddSingleton<IIndexProvider, MapTilesIndexProvider>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "Pois.BuildEditor",
                areaName: "Etch.OrchardCore.Leaflet",
                pattern: _adminOptions.AdminUrlPrefix + "/Pois/BuildEditor",
                defaults: new { controller = typeof(AdminController).ControllerName(), action = nameof(AdminController.BuildEditor) }
            );
        }
    }
}