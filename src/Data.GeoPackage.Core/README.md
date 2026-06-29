# Altemiq.Data.GeoPackage.Core

A core library for GeoPackage database operations. GeoPackage is an open, standard container for geographic information.

## Features

- **GeoPackage specification compliance**: Full support for OGC GeoPackage standard
- **Geometry column support**: Read/write geometry data in GeoPackage tables
- **Spatial reference system support**: Coordinate system metadata management
- **Schema management**: Create and modify GeoPackage schemas

## Usage

```csharp
using Altemiq.Data.GeoPackage;

// Create a GeoPackage connection
var connection = new GeoPackageConnection("data.gpkg");

// Get geometry from a feature
var geometry = reader.GetGeometry("geometry");

// Get spatial reference information
var srs = connection.GetSpatialReferenceSystem(4326);
```

## Target Frameworks

- .NET 6.0
- .NET Standard 2.1
- .NET Standard 2.0

## Dependencies

- [Altemiq.Data.Geometry](https://www.nuget.org/packages/Altemiq.Data.Geometry/) - Geometry data layer
- [Altemiq.Data.Sqlite.Core](https://www.nuget.org/packages/Altemiq.Data.Sqlite/) - SQLite core functionality

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
