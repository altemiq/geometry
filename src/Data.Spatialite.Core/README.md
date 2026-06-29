# Altemiq.Data.Spatialite.Core

A core library for SpatiaLite database operations. SpatiaLite extends SQLite with spatial capabilities.

## Features

- **Spatial operations**: Full SpatiaLite spatial functions
- **Geometry initialization**: Initialize spatial metadata in databases
- **Spatial reference system support**: Coordinate system management
- **High-performance**: Span-based parsing for geometry data

## Usage

```csharp
using Altemiq.Data.Spatialite;

// Create a SpatiaLite connection
var connection = new SpatialiteConnection("data.db");

// Initialize spatial metadata
connection.InitSpatialMetadata(SpatialMetadataMode.Wgs84);

// Execute spatial query
using var command = connection.CreateCommand(
    "SELECT * FROM features WHERE ST_Intersects(geometry, BuildCircleMbr(0, 0, 1000))"
);
```

## Target Frameworks

- .NET 6.0
- .NET Standard 2.1
- .NET Standard 2.0

## Dependencies

- [Altemiq.Data.Sqlite.Core](https://www.nuget.org/packages/Altemiq.Data.Sqlite/) - SQLite core functionality
- [SpatiaLite](https://www.nuget.org/packages/SpatiaLite/) - SpatiaLite native library
- [Microsoft.Data.Sqlite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/) - SQLite data provider
- [Altemiq.Runtime](https://www.nuget.org/packages/Altemiq.Runtime/) - Runtime utilities
- [Microsoft.Extensions.DependencyModel](https://www.nuget.org/packages/Microsoft.Extensions.DependencyModel/) - Dependency resolution

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
