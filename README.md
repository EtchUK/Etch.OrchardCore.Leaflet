# Etch.OrchardCore.Leaflet

Module for [Orchard Core](https://github.com/OrchardCMS/OrchardCore) that provides interactive maps using [Leaflet](https://leafletjs.com/).

## Build Status

[![Build Status](https://secure.travis-ci.org/etchuk/Etch.OrchardCore.Leaflet.png?branch=master)](http://travis-ci.org/etchuk/Etch.OrchardCore.Leaflet) [![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.Leaflet.svg)](https://www.nuget.org/packages/Etch.OrchardCore.Leaflet)

## Orchard Core Reference

This module is referencing a stable build of Orchard Core ([`1.1.0`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.1.0)).

## OS Support

This module is limited to Windows because it's using [Deep Zoom](https://www.microsoft.com/silverlight/deep-zoom/) library for tile generation.

## Installing

This module is available on [NuGet](https://www.nuget.org/packages/Etch.OrchardCore.Leaflet). Add a reference to your Orchard Core web project via the NuGet package manager. Search for "Etch.OrchardCore.Leaflet", ensuring include prereleases is checked.

The NuGet package for this module includes an application that handles the tile generator. To integrate this appliation with the module there needs to be a couple of ammendments to the project's `.csproj` file. Firstly, the `PackageReference` that was added needs to have an additional `GeneratePathProperty` property added as shown below.

```
<PackageReference Include="Etch.OrchardCore.Leaflet" Version="0.1.0-rc1" GeneratePathProperty="true" />
```

The other change is to copy the tile generator application files from the NuGet package in to the output directory of the project. Add the following to the `.csproj` file that will handle copying files from the NuGet package in to the output directory.

```
<Target Name="CopyTileGenerator" AfterTargets="Build">
    <Copy SourceFiles="$(PkgEtch_OrchardCore_Leaflet)\content\TileGenerator\DeepZoomTools.dll" DestinationFolder="$(OutDir)/TileGenerator" />
    <Copy SourceFiles="$(PkgEtch_OrchardCore_Leaflet)\content\TileGenerator\Etch.OrchardCore.LeafletTileGenerator.exe" DestinationFolder="$(OutDir)/TileGenerator" />
    <Copy SourceFiles="$(PkgEtch_OrchardCore_Leaflet)\content\TileGenerator\Etch.OrchardCore.LeafletTileGenerator.exe.config" DestinationFolder="$(OutDir)/TileGenerator" />
</Target>

<Target Name="PublishTileGenerator" AfterTargets="AfterPublish">
    <Copy SourceFiles="$(PkgEtch_OrchardCore_Leaflet)\content\TileGenerator\DeepZoomTools.dll" DestinationFolder="$(PublishDir)/TileGenerator" />
    <Copy SourceFiles="$(PkgEtch_OrchardCore_Leaflet)\content\TileGenerator\Etch.OrchardCore.LeafletTileGenerator.exe" DestinationFolder="$(PublishDir)/TileGenerator" />
    <Copy SourceFiles="$(PkgEtch_OrchardCore_Leaflet)\content\TileGenerator\Etch.OrchardCore.LeafletTileGenerator.exe.config" DestinationFolder="$(PublishDir)/TileGenerator" />
</Target>
```

## Usage

_Module is still being developed, usage instructions will be available in the future._
