# Altemiq.Text.GeoJson

A library for reading and writing GeoJSON format using System.Text.Json for high-performance JSON serialization.

## Features

- **GeoJSON parsing**: Parse GeoJSON Feature, FeatureCollection, and geometry objects
- **GeoJSON formatting**: Serialize geometry and features to GeoJSON format
- **High-performance**: Uses System.Text.Json for fast JSON processing
- **Nullable support**: Full nullable reference type support
- **Custom converters**: Specialized converters for geometry types

## Usage

```csharp
using Altemiq.Text.GeoJson;

// Parse GeoJSON
var geojson = """
{
  "type": "Feature",
  "geometry": {
    "type": "Point",
    "coordinates": [10, 20]
  },
  "properties": {
    "name": "Test"
  }
}
""";
var feature = GeoJsonConverter.Parse<Feature>(geojson);

// Serialize to GeoJSON
var output = GeoJsonConverter.ToString(feature);
```

## GeoJSON Format

GeoJSON is a geospatial data interchange format based on JavaScript Object Notation (JSON). Supported types:

- Point, MultiPoint, LineString, MultiLineString, Polygon, MultiPolygon
- Feature, FeatureCollection
- GeometryCollection

## Target Frameworks

- .NET Standard 2.1
- .NET Standard 2.0
- .NET Framework 4.61

## Dependencies

- [Altemiq.Geometry](https://www.nuget.org/packages/Altemiq.Geometry/) - Core geometry types
- [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/) - JSON serialization

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
