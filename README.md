# Altemiq.Geometry

[![Build Status](https://github.com/altemiq/geometry/actions/workflows/build.yml/badge.svg)](https://github.com/altemiq/geometry/actions/workflows/build.yml)
[![License](https://img.shields.io/github/license/altemiq/geometry)](https://github.com/altemiq/geometry/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Altemiq.Geometry.svg)](https://www.nuget.org/packages/Altemiq.Geometry/)

A comprehensive .NET library for working with geospatial data, providing:

- **Core geometry types**: Point, Polygon, Polyline, and multi-variants with Z (elevation) and M (measure) support
- **Data format parsers**: DBF, GeoPackage, SpatiaLite, SQLite
- **I/O formats**: ESRI Shapefile, MapInfo TAB/MIF, GeoJSON, WKT, WKB
- **Protobuf serialization**: Efficient binary serialization for geometry data
- **gRPC support**: Protocol Buffers and gRPC integration

## Installation

Install the core library:

```bash
dotnet add package Altemiq.Geometry
```

Install format-specific packages:

```bash
# Shapefile I/O
dotnet add package Altemiq.IO.Geometry.Shapefile

# MapInfo TAB I/O
dotnet add package Altemiq.IO.Geometry.MapInfo

# GeoPackage support
dotnet add package Altemiq.Data.GeoPackage

# SpatiaLite support
dotnet add package Altemiq.Data.Spatialite

# GeoJSON support
dotnet add package Altemiq.Text.GeoJson

# STAC support
dotnet add package Altemiq.Text.GeoJson.Stac

# Protobuf serialization
dotnet add package Altemiq.Geometry.Protobuf
```

## Quick Start

```csharp
using Altemiq.Geometry;
using Altemiq.IO.Geometry;
using Altemiq.Text.GeoJson;

// Create a geometry
var point = new Point(10, 20);

// Parse WKT
var geometry = GeometryConverter.Parse("POINT(10 20)");

// Parse GeoJSON
var geojson = """
{
  "type": "Feature",
  "geometry": {
    "type": "Point",
    "coordinates": [10, 20]
  }
}
""";
var feature = GeoJsonConverter.Parse<Feature>(geojson);

// Read Shapefile
using var reader = new ShapefileReader("data.shp");
while (reader.Read())
{
    var geom = reader.GetGeometry();
    var attrs = reader.GetAttributes();
}
```

## Architecture

### Core Library

- **[Altemiq.Geometry](src/Geometry/README.md)**: Core geometry types and operations

### Data Packages

- **[Altemiq.Data.Dbf](src/Data.Dbf/README.md)**: dBASE III and Visual FoxPro DBF file support
- **[Altemiq.Data.Geometry](src/Data.Geometry/README.md)**: Geometry data layer interfaces
- **[Altemiq.Data.Sqlite](src/Data.Sqlite/README.md)**: SQLite data provider (with native binaries)
- **[Altemiq.Data.Sqlite.Core](src/Data.Sqlite.Core/README.md)**: SQLite core functionality
- **[Altemiq.Data.GeoPackage](src/Data.GeoPackage/README.md)**: GeoPackage data provider
- **[Altemiq.Data.GeoPackage.Core](src/Data.GeoPackage.Core/README.md)**: GeoPackage core functionality
- **[Altemiq.Data.Spatialite](src/Data.Spatialite/README.md)**: SpatiaLite data provider
- **[Altemiq.Data.Spatialite.Core](src/Data.Spatialite.Core/README.md)**: SpatiaLite core functionality

### I/O Packages

- **[Altemiq.IO.Geometry](src/IO.Geometry/README.md)**: WKT/WKB geometry I/O
- **[Altemiq.IO.Geometry.Shapefile](src/IO.Geometry.Shapefile/README.md)**: ESRI Shapefile I/O
- **[Altemiq.IO.Geometry.MapInfo](src/IO.Geometry.MapInfo/README.md)**: MapInfo TAB I/O
- **[Altemiq.IO.Geometry.Spatialite](src/IO.Geometry.Spatialite/README.md)**: SpatiaLite geometry I/O
- **[Altemiq.IO.Geometry.Spatialite.Core](src/IO.Geometry.Spatialite.Core/README.md)**: SpatiaLite I/O core
- **[Altemiq.IO.Geodesy](src/IO.Geodesy/README.md)**: Coordinate system WKT parsing

### Serialization Packages

- **[Altemiq.Text.GeoJson](src/Text.GeoJson/README.md)**: GeoJSON serialization
- **[Altemiq.Text.GeoJson.Stac](src/Text.GeoJson.Stac/README.md)**: STAC catalog serialization
- **[Altemiq.Geometry.Protobuf](src/Geometry.Protobuf/README.md)**: Protocol Buffers serialization

### Build Tools

- **[Altemiq.Grpc.Geometry.Tools](src/Grpc.Geometry.Tools/README.md)**: Build-time gRPC/Protobuf tools

## Target Frameworks

Most libraries support:
- .NET 7.0+
- .NET Standard 2.0+
- .NET Framework 4.61+ (where applicable)

## Building

```bash
dotnet build
dotnet test
```

## Documentation

- [DBF format specification](docs/xbase/dbf.md)
- [ESRI Shapefile specification](docs/esri/shapefile.md)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please read our contributing guidelines before submitting pull requests.
