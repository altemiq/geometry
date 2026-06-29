# Altemiq.Geometry.Protobuf

A library for serializing geometry data using Google Protocol Buffers (protobuf). Provides converters between geometry types and protobuf messages.

## Features

- **Protobuf serialization**: Convert geometry to/from protobuf messages
- **Multiple format support**: WKT, WKB, GeoJSON, ESRI Shape formats
- **Spatial reference support**: Serialize projection information
- **Envelope support**: Serialize bounding box information
- **High-performance**: Efficient binary serialization

## Usage

```csharp
using Altemiq.Geometry.Protobuf;
using Altemiq.Protobuf.WellKnownTypes;

// Parse protobuf geometry
var protobufGeometry = new GeometryData
{
    Wkt = "POINT(10 20)"
};
var geometry = GeometryConverter.Parse(protobufGeometry);

// Convert to protobuf
var protobuf = GeometryConverter.ToProtobuf(geometry);
```

## Protobuf Schema

The protobuf schema defines several message types:

- **GeometryData**: Container for geometry with WKT/WKB/GeoJSON/ESRI Shape formats
- **ProjectionData**: Spatial reference system definition
- **EnvelopeData**: Bounding box coordinates

## Target Frameworks

- .NET Standard 2.1
- .NET Standard 2.0

## Dependencies

- [Google.Protobuf](https://www.nuget.org/packages/Google.Protobuf/) - Protocol Buffers runtime
- [Altemiq.Text.GeoJson](https://www.nuget.org/packages/Altemiq.Text.GeoJson/) - GeoJSON support

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
