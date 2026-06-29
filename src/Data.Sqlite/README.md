# Altemiq.Data.Sqlite

A SQLite data provider package that includes native SQLite binaries for easy deployment. This is a wrapper package that depends on `Altemiq.Data.Sqlite.Core`.

## Features

- **Native binaries included**: All required SQLite native libraries included
- **Cross-platform support**: Works on Windows, Linux, and macOS
- **Easy deployment**: No need to manually manage native dependencies

## Usage

```csharp
using Altemiq.Data.Sqlite;

// Use the connection - native binaries are automatically loaded
var connection = new SqliteConnection("Data Source=database.db");
connection.Open();
```

## Target Frameworks

- .NET Standard 2.0

## Dependencies

- [Altemiq.Data.Sqlite.Core](https://www.nuget.org/packages/Altemiq.Data.Sqlite/) - Core SQLite functionality
- [Microsoft.Data.Sqlite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/) - SQLite data provider

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
