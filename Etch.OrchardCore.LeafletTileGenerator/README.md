# Leaflet Tile Generator

.NET framework console application that generates tiles using [Deep Zoom](https://www.microsoft.com/silverlight/deep-zoom/).

## Usage

Console application takes two arguments, first is the destination of the generated tiles and the second is the path to the image that needs to be converted to tiles.

```
./Etch.OrchardCore.LeafletTileGenerator.exe C:\destination\example-map C:\source\example-map.jpg
```

## Build

Project will place it's build artifacts in `Etch.OrchardCore.Leaflet\TileGenerator`.
