# Altemiq.Text.GeoJson.Stac

A library for reading and writing SpatioTemporal Asset Catalog (STAC) format using System.Text.Json. STAC extends GeoJSON for cataloging geospatial assets.

## Features

- **STAC parsing**: Parse STAC Catalog, Collection, and Item objects
- **STAC formatting**: Serialize STAC objects to JSON format
- **Temporal extent support**: Handle time ranges and single timestamps
- **Spatial extent support**: Define bounding boxes and geometry
- **Asset management**: Link and manage geospatial assets

## Usage

```csharp
using Altemiq.Text.GeoJson.Stac;

// Parse STAC Item
var stacItem = """
{
  "type": "Feature",
  "stac_version": "1.0.0",
  "stac_extensions": ["eo"],
  "geometry": {
    "type": "Point",
    "coordinates": [10, 20]
  },
  "properties": {
    "datetime": "2023-01-01T00:00:00Z"
  },
  "assets": {
    "data": {
      "href": "https://example.com/data.tif"
    }
  }
}
""";
var item = StacConverter.Parse<Item>(stacItem);

// Serialize to STAC
var output = StacConverter.ToString(item);
```

## STAC Format

SpatioTemporal Asset Catalog (STAC) is a specification for cataloging geospatial assets. Key components:

- **Catalog**: A container of collections and other catalogs
- **Collection**: A set of items that share common properties
- **Item**: A single geospatial asset with metadata

## Target Frameworks

- .NET Standard 2.1
- .NET Standard 2.0
- .NET Framework 4.61

## Dependencies

- [Altemiq.Text.GeoJson](https://www.nuget.org/packages/Altemiq.Text.GeoJson/) - GeoJSON support

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
