# Altemiq.Data.Geometry

A data layer for geometry operations, providing interfaces for reading and writing geometry data from databases and other data sources.

## Features

- **IDataRecord extensions**: Geometry-aware `IDataRecord` interface
- **Geometry data readers**: High-performance geometry data reading
- **Integration with core library**: Works seamlessly with `Altemiq.Geometry`

## Usage

```csharp
using Altemiq.Data.Geometry;

// Read geometry from a data reader
var geometry = reader.GetGeometry("SHAPE");

// Write geometry to a data record
record.SetGeometry("SHAPE", geometry);
```

## Target Frameworks

- .NET Standard 2.0

## Dependencies

- [Altemiq.Geometry](https://www.nuget.org/packages/Altemiq.Geometry/) - Core geometry types
- [System.Memory](https://www.nuget.org/packages/System.Memory/) - For netstandard2.0 compatibility

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
