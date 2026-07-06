# Altemiq.Geometry

A high-performance geometry library for .NET that provides strongly-typed geometry types with support for 2D, 3D (Z), and measured (M) coordinates.

## Features

- **Strongly typed geometry types**: Point, Polygon, Polyline, and their multi-variants
- **Span-based parsing**: High-performance WKT/WKB parsing using `ReadOnlySpan<T>`
- **Nullable reference types**: Full nullable annotation support
- **Target frameworks**: .NET 7.0+ and .NET Standard 2.0

## Usage

```csharp
using Altemiq.Geometry;

// Create a point
var point = new Point(10, 20);

// Create a polygon
var polygon = new Polygon(
    new LinearRing(new[]
    {
        new Point(0, 0),
        new Point(10, 0),
        new Point(10, 10),
        new Point(0, 10),
        new Point(0, 0)
    })
);

// Parse WKT
var geometry = GeometryConverter.Parse("POINT(10 20)");
```

## Target Frameworks

- .NET 7.0
- .NET Standard 2.0

## Dependencies

- [System.Memory](https://www.nuget.org/packages/System.Memory/) - For netstandard2.0 compatibility
- [System.Runtime.CompilerServices.Unsafe](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/) - For netstandard2.0 compatibility
- [Microsoft.Bcl.HashCode](https://www.nuget.org/packages/Microsoft.Bcl.HashCode/) - For netstandard2.0 compatibility

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
