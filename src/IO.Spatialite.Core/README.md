# Altemiq.IO.Spatialite.Core

A core library for reading and writing geometry data in SpatiaLite format. This package provides the foundational types for SpatiaLite I/O operations.

## Features

- **SpatiaLite geometry reading**: Read geometry data from SpatiaLite databases
- **Gaia geometry writer**: Write geometry data using SpatiaLite's Gaia interface
- **SpatialiteRecord**: High-performance geometry record handling
- **Integration with core library**: Works with `Altemiq.Data.Spatialite.Core`

## Usage

```csharp
using Altemiq.IO.Spatialite;

// Read geometry from SpatiaLite
using var reader = new SpatialiteReader(connection, "features", "geometry");
while (reader.Read())
{
    var geometry = reader.GetGeometry();
}

// Write geometry to SpatiaLite using Gaia
using var writer = new GaiaWriter(connection, "features", "geometry");
writer.Write(geometry);
```

## Target Frameworks

- .NET 6.0
- .NET Standard 2.1
- .NET Standard 2.0

## Dependencies

- [Altemiq.Data.Geometry](https://www.nuget.org/packages/Altemiq.Data.Geometry/) - Geometry data layer
- [Altemiq.Data.Spatialite.Core](https://www.nuget.org/packages/Altemiq.Data.Spatialite/) - SpatiaLite core

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
