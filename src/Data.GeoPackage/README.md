# Altemiq.Data.GeoPackage

A complete GeoPackage data provider package that includes native SQLite binaries. This is a wrapper package that depends on `Altemiq.Data.GeoPackage.Core`.

## Features

- **Native binaries included**: All required SQLite native libraries included
- **GeoPackage support**: Full OGC GeoPackage standard compliance
- **Cross-platform**: Works on Windows, Linux, and macOS
- **Easy deployment**: No manual native dependency management

## Usage

```csharp
using Altemiq.Data.GeoPackage;

// Open a GeoPackage
using var connection = new GeoPackageConnection("data.gpkg");
connection.Open();

// Read features
using var command = connection.CreateCommand("SELECT * FROM features");
using var reader = command.ExecuteReader();
while (reader.Read())
{
    var geometry = reader.GetGeometry("geometry");
}
```

## Target Frameworks

- .NET Standard 2.0

## Dependencies

- [Altemiq.Data.GeoPackage.Core](https://www.nuget.org/packages/Altemiq.Data.GeoPackage/) - Core GeoPackage functionality
- [Microsoft.Data.Sqlite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/) - SQLite data provider

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
