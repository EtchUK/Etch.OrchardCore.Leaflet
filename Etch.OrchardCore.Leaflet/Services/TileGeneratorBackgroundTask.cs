using Etch.OrchardCore.Leaflet.Indexes;
using Etch.OrchardCore.Leaflet.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Entities;
using OrchardCore.Environment.Shell;
using OrchardCore.Media;
using OrchardCore.Media.Fields;
using OrchardCore.Settings;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YesSql;

namespace Etch.OrchardCore.Leaflet.Services
{
    [BackgroundTask(Schedule = "* * * * *", Description = "Generates tiles for map")]
    public class TileGeneratorBackgroundTask : IBackgroundTask
    {
        #region Constants

        private const string LeafletCachePath = "leaflet-cache";

        #endregion

        #region Dependencies

        private readonly ILogger<TileGeneratorBackgroundTask> _logger;
        private readonly IOptions<ShellOptions> _shellOptions;
        private readonly ShellSettings _shellSettings;

        #endregion

        #region Constructor

        public TileGeneratorBackgroundTask(
            ILogger<TileGeneratorBackgroundTask> logger, 
            IOptions<ShellOptions> shellOptions,
            ShellSettings shellSettings)
        {
            _logger = logger;
            _shellOptions = shellOptions;
            _shellSettings = shellSettings;
        }

        #endregion

        #region Implementation

        public async Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var itemToProcess = await GetNextToProcessAsync(serviceProvider);

            if (itemToProcess == null)
            {
                return;
            }

            _logger.LogInformation($"Generating tiles for {itemToProcess.DisplayText}");

            await UpdateToProcessingAsync(serviceProvider, itemToProcess);

            _logger.LogInformation($"Updated content item to processing");

            var mediaField = itemToProcess.As<MapTiles>().Get<MediaField>(Constants.TilesMediaFileFieldName);

            if (mediaField == null || !mediaField.Paths.Any())
            {
                _logger.LogInformation($"Unable to find map tile media");
                return;
            }

            var mapImagePath = await CopyFileToCacheFolder(serviceProvider, itemToProcess, mediaField.Paths[0]);

            GenerateTiles(mapImagePath);
            await MoveFilesToMediaStoreAsync(serviceProvider, itemToProcess, mapImagePath, mediaField.Paths[0]);
            await UpdateToProcessedAsync(serviceProvider, itemToProcess);
            DeleteCache(mapImagePath);
        }

        #endregion

        #region Helper Methods

        private string CacheDirectory => PathExtensions.Combine(
            _shellOptions.Value.ShellsApplicationDataPath,
            _shellOptions.Value.ShellsContainerName,
            _shellSettings.Name, LeafletCachePath);

        private async Task<string> CopyFileToCacheFolder(IServiceProvider serviceProvider, ContentItem contentItem, string mapImagePath)
        {
            _logger.LogInformation($"Copying file from media library to local file system");

            var mediaStore = serviceProvider.GetRequiredService<IMediaFileStore>();
            var path = PathExtensions.Combine(CacheDirectory, contentItem.ContentItemId);
            var fileStream = await mediaStore.GetFileStreamAsync(mapImagePath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var tempFile = File.Create(Path.Combine(path, Path.GetFileName(mapImagePath)));
            fileStream.CopyTo(tempFile);
            tempFile.Close();

            return Path.Combine(path, Path.GetFileName(mapImagePath));
        }

        private void DeleteCache(string cachePath)
        {
            _logger.LogInformation($"Deleting files from local file system");

            if (Directory.Exists(Path.GetDirectoryName(cachePath)))
            {
                Directory.Delete(Path.GetDirectoryName(cachePath), true);
            }
        }

        private void GenerateTiles(string path)
        {
            _logger.LogInformation($"Generating tiles with external process");

            using (var process = new Process())
            {
                process.StartInfo.FileName = $"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TileGenerator", "Etch.OrchardCore.LeafletTileGenerator.exe")}";
                process.StartInfo.Arguments = $"{Path.GetDirectoryName(path)} {path}";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.OutputDataReceived += (sender, data) => _logger.LogDebug(data.Data);
                process.ErrorDataReceived += (sender, data) => _logger.LogError(data.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        } 

        private async Task<ContentItem> GetNextToProcessAsync(IServiceProvider serviceProvider)
        {
            return await serviceProvider
                .GetRequiredService<ISession>()
                .Query<ContentItem>()
                .With<MapTilesIndex>(x => !x.HasBeenProcessed)
                .With<ContentItemIndex>()
                .OrderBy(x => x.CreatedUtc)
                .FirstOrDefaultAsync();
        }

        private async Task MoveFilesToMediaStoreAsync(IServiceProvider serviceProvider, ContentItem contentItem, string cachePath, string mediaStorePath)
        {
            _logger.LogInformation($"Moving files from local file system to media store");

            var mediaStore = serviceProvider.GetRequiredService<IMediaFileStore>();
            var cachedTilesPath = Path.GetDirectoryName(cachePath) + "_files";
            var zoomLevels = Directory.EnumerateDirectories(cachedTilesPath).Count();
            var currentZoomLevel = 0;

            while (currentZoomLevel < zoomLevels)
            {
                if (!Directory.Exists(Path.Combine(cachedTilesPath, currentZoomLevel.ToString()))) {
                    break;
                }

                _logger.LogInformation($"Moving files at zoom level {currentZoomLevel}");

                foreach (var tile in Directory.GetFiles(Path.Combine(cachedTilesPath, currentZoomLevel.ToString()))) {
                    await mediaStore.CreateFileFromStreamAsync(Path.Combine(Path.GetDirectoryName(mediaStorePath), contentItem.ContentItemId, currentZoomLevel.ToString(), Path.GetFileName(tile)), File.OpenRead(tile), true);
                }

                currentZoomLevel++;
            }
        }

        private async Task UpdateToProcessedAsync(IServiceProvider serviceProvider, ContentItem contentItem)
        {
            var contentManager = serviceProvider.GetRequiredService<IContentManager>();

            _logger.LogInformation($"Update content item to reflect tiles have been generated");

            var mapTiles = contentItem.As<MapTiles>();
            mapTiles.HasBeenProcessed = true;
            mapTiles.IsProcessing = false;
            mapTiles.Apply();

            if (mapTiles.PublishAfterProcessed)
            {
                await contentManager.PublishAsync(contentItem);
                return;
            }

            await contentManager.UpdateAsync(contentItem);
        }

        private async Task UpdateToProcessingAsync(IServiceProvider serviceProvider, ContentItem contentItem)
        {
            var contentManager = serviceProvider.GetRequiredService<IContentManager>();

            var mapTiles = contentItem.As<MapTiles>();
            mapTiles.HasBeenProcessed = true;
            mapTiles.IsProcessing = true;
            mapTiles.Apply();

            await contentManager.UpdateAsync(contentItem);
        }

        #endregion
    }
}
