# Altemiq.IO.Spatialite

A complete SpatiaLite I/O package that includes native SQLite and SpatiaLite binaries. This is a wrapper package that depends on `Altemiq.IO.Spatialite.Core`.

## Features

- **Native binaries included**: SQLite and SpatiaLite native libraries included
- **SpatiaLite geometry I/O**: Read/write geometry data in SpatiaLite databases
- **Cross-platform**: Works on Windows, Linux, and macOS
- **Easy deployment**: No manual native dependency management

## Usage

```csharp
using Altemiq.IO.Spatialite;

// Open a SpatiaLite database
using var connection = new SpatialiteConnection("data.db");
connection.Open();

// Read geometry from SpatiaLite
using var reader = new SpatialiteReader(connection, "features", "geometry");
while (reader.Read())
{
    var geometry = reader.GetGeometry();
}

// Write geometry to SpatiaLite
using var writer = new GaiaWriter(connection, "features", "geometry");
writer.Write(geometry);
```

## Target Frameworks

- .NET Standard 2.0

## Dependencies

- [Altemiq.IO.Spatialite.Core](https://www.nuget.org/packages/Altemiq.IO.Spatialite/) - Core SpatiaLite I/O
- [Microsoft.Data.Sqlite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/) - SQLite data provider

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
