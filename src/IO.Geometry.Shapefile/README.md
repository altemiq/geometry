# Altemiq.IO.Geometry.Shapefile

A library for reading and writing ESRI Shapefile format (.shp, .shx, .dbf) with support for projection definitions.

## Features

- **Shapefile reading**: Support for all ESRI Shapefile geometry types
- **Shapefile writing**: Create new Shapefiles with geometry and attributes
- **Projection support**: Embedded ESRI projection database (pe_list_geogcs.json, pe_list_projcs.json)
- **DBF integration**: Reads associated .dbf files for attributes
- **QIX spatial indexing**: Support for spatial index files

## Usage

```csharp
using Altemiq.IO.Geometry.Shapefile;

// Read a Shapefile
using var reader = new ShapefileReader("data.shp");
while (reader.Read())
{
    var geometry = reader.GetGeometry();
    var attributes = reader.GetAttributes();
}

// Write a Shapefile
using var writer = new ShapefileWriter("output.shp", ShapeType.Point);
writer.Write(new ShapefileRecord(geometry, attributes));
```

## Shapefile Format

The Shapefile format consists of multiple files:
- `.shp` - Main file storing geometry
- `.shx` - Index file for fast record access
- `.dbf` - dBASE file for attributes
- `.prj` - Projection definition (optional)
- `.qix` - Spatial index (optional)

## Target Frameworks

- .NET Standard 2.1
- .NET Standard 2.0
- .NET Framework 4.61

## Dependencies

- [Altemiq.Data.Dbf](https://www.nuget.org/packages/Altemiq.Data.Dbf/) - DBF file support
- [Altemiq.IO.Geodesy](https://www.nuget.org/packages/Altemiq.IO.Geodesy/) - Coordinate system support
- [Altemiq.IO.Geometry](https://www.nuget.org/packages/Altemiq.IO.Geometry/) - Geometry I/O
- [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/) - JSON serialization

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
