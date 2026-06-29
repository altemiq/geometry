# Altemiq.IO.Geometry

A library for reading and writing geometry data in various formats including WKT (Well-Known Text) and WKB (Well-Known Binary).

## Features

- **WKT parsing and formatting**: High-performance text-based geometry representation
- **WKB parsing and formatting**: Binary geometry format for efficient storage
- **EWKT support**: Extended WKT with SRID information
- **Span-based operations**: High-performance operations using `ReadOnlySpan<T>`

## Usage

```csharp
using Altemiq.IO.Geometry;

// Parse WKT
var geometry = GeometryConverter.Parse("POINT(10 20)");

// Format to WKT
var wkt = geometry.ToWkt();

// Parse WKB
var wkb = Convert.FromBase64String("AAAAAAABFAAAAAAAgP8AAAAAAAAAAIB/AAAAAAAAgH8=");
var geometryFromWkb = GeometryConverter.Parse(wkb);

// Write to WKB
var outputWkb = geometry.ToWkb();
```

## Target Frameworks

- .NET 7.0
- .NET Standard 2.1
- .NET Standard 2.0
- .NET Framework 4.61

## Dependencies

- [Altemiq.Data.Geometry](https://www.nuget.org/packages/Altemiq.Data.Geometry/) - Geometry data layer
- [System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple/) - For .NET Framework compatibility
- [Microsoft.Bcl.AsyncInterfaces](https://www.nuget.org/packages/Microsoft.Bcl.AsyncInterfaces/) - For netstandard2.0 compatibility

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
