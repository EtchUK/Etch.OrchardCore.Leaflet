using Etch.OrchardCore.Leaflet.Drivers;
using Etch.OrchardCore.Leaflet.Indexes;
using Etch.OrchardCore.Leaflet.Models;
using Etch.OrchardCore.Leaflet.Services;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using YesSql.Indexes;

namespace Etch.OrchardCore.Leaflet
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentPart<MapTiles>();
            services.AddScoped<IContentPartDisplayDriver, MapTilesDisplay>();

            services.AddContentPart<Map>();
            services.AddScoped<IContentPartDisplayDriver, MapDisplay>();

            services.AddScoped<IDataMigration, Migrations>();

            services.AddSingleton<IBackgroundTask, TileGeneratorBackgroundTask>();
            services.AddSingleton<IIndexProvider, MapTilesIndexProvider>();
        }
    }
}