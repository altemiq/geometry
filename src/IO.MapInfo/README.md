# Altemiq.IO.MapInfo

A library for reading and writing MapInfo TAB and MIF/WIL formats with support for MapInfo-specific features.

## Features

- **TAB file reading**: Read MapInfo TAB files with geometry and attributes
- **MapInfo geometry types**: Support for MapInfo-specific geometry types
- **Table type detection**: Automatically detect table types (Simple, Interchange, etc.)
- **DBF integration**: Reads associated DBF files for attributes

## Usage

```csharp
using Altemiq.IO.MapInfo;

// Read a MapInfo TAB file
using var reader = new MapInfoReader("data.tab");
while (reader.Read())
{
    var geometry = reader.GetGeometry();
    var attributes = reader.GetAttributes();
}
```

## MapInfo Format

MapInfo supports multiple file formats:
- `.tab` - Main TAB file with reference to associated files
- `.dat` - DBF file for attributes
- `.map` - Binary geometry file
- `.id` - Record ID file

## Target Frameworks

- .NET Standard 2.1
- .NET Standard 2.0
- .NET Framework 4.61

## Dependencies

- [Altemiq.Data.Dbf](https://www.nuget.org/packages/Altemiq.Data.Dbf/) - DBF file support
- [Altemiq.Data.Geometry](https://www.nuget.org/packages/Altemiq.Data.Geometry/) - Geometry data layer
- [Altemiq.Geometry](https://www.nuget.org/packages/Altemiq.Geometry/) - Core geometry types

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
