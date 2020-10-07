using Etch.OrchardCore.Leaflet.Drivers;
using Etch.OrchardCore.Leaflet.Models;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Etch.OrchardCore.Leaflet
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentPart<MapTiles>();
            services.AddScoped<IContentPartDisplayDriver, MapTilesDisplay>();

            services.AddScoped<IDataMigration, Migrations>();
        }
    }
}