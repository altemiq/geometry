# Altemiq.Data.Spatialite

A complete SpatiaLite data provider package that includes native SQLite and SpatiaLite binaries. This is a wrapper package that depends on `Altemiq.Data.Spatialite.Core`.

## Features

- **Native binaries included**: SQLite and SpatiaLite native libraries included
- **Spatial operations**: Full SpatiaLite spatial function support
- **Cross-platform**: Works on Windows, Linux, and macOS
- **Easy deployment**: No manual native dependency management

## Usage

```csharp
using Altemiq.Data.Spatialite;

// Open a SpatiaLite database
using var connection = new SpatialiteConnection("data.db");
connection.Open();

// Initialize spatial metadata
connection.InitSpatialMetadata(SpatialMetadataMode.Wgs84);

// Query spatial data
using var command = connection.CreateCommand(
    "SELECT * FROM features WHERE ST_Distance(geometry, BuildCircleMbr(0, 0, 1000)) < 500"
);
using var reader = command.ExecuteReader();
```

## Target Frameworks

- .NET Standard 2.0

## Dependencies

- [Altemiq.Data.Spatialite.Core](https://www.nuget.org/packages/Altemiq.Data.Spatialite/) - Core SpatiaLite functionality
- [Microsoft.Data.Sqlite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/) - SQLite data provider
- [SpatiaLite](https://www.nuget.org/packages/SpatiaLite/) - SpatiaLite native library

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
