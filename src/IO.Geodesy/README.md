# Altemiq.IO.Geodesy

A library for geodetic coordinate system operations, including Well-Known Text (WKT) parsing and formatting for spatial reference systems.

## Features

- **WKT parsing**: Parse coordinate system definitions from WKT format
- **WKT formatting**: Format coordinate systems as WKT strings
- **Spatial reference system support**: Handle EPSG codes, PROJ strings, and custom projections
- **High-performance**: Span-based parsing for efficient WKT processing

## Usage

```csharp
using Altemiq.IO.Geodesy;

// Parse WKT coordinate system
var wkt = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563]],PRIMEM[\"Greenwich\",0],UNIT[\"degree\",0.0174532925199433]]";
var srs = WellKnownTextParser.Parse(wkt);

// Format to WKT
var outputWkt = srs.ToWkt();

// Parse EPSG code
var srsFromEpsg = WellKnownTextParser.Parse(4326);
```

## WKT Format

Well-Known Text (WKT) is a text markup language for representing coordinate reference systems. Examples:

```wkt
GEOGCS["WGS 84",
    DATUM["WGS_1984",
        SPHEROID["WGS 84",6378137,298.257223563]],
    PRIMEM["Greenwich",0],
    UNIT["degree",0.0174532925199433]]
```

## Target Frameworks

- .NET 6.0
- .NET Standard 2.1
- .NET Standard 2.0
- .NET Framework 4.61

## Dependencies

- [System.Memory](https://www.nuget.org/packages/System.Memory/) - For netstandard2.0 compatibility
- [Microsoft.Bcl.HashCode](https://www.nuget.org/packages/Microsoft.Bcl.HashCode/) - For netstandard2.0 compatibility

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
