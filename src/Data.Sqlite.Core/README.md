# Altemiq.Data.Sqlite.Core

A core library for SQLite database operations with enhanced schema metadata support. This package provides the foundational types used by other data packages.

## Features

- **Enhanced schema reading**: Extended metadata collection for SQLite databases
- **Filtered table schema**: Retrieve specific tables with custom filters
- **Integration with core data types**: Works with `Altemiq.Data.Geometry`
- **Span-based performance**: High-performance operations using `ReadOnlySpan<T>`

## Usage

```csharp
using Altemiq.Data.Sqlite;

// Create a connection
var connection = new SqliteConnection("Data Source=database.db");

// Get table schema
var schema = connection.GetTableSchema("my_table");

// Get filtered schema
var filtered = connection.GetFilteredTableSchema(
    0, 
    null, 
    null, 
    "alias", 
    "table"
);
```

## Target Frameworks

- .NET 6.0
- .NET Standard 2.1
- .NET Standard 2.0

## Dependencies

- [Microsoft.Data.Sqlite.Core](https://www.nuget.org/packages/Microsoft.Data.Sqlite.Core/) - SQLite data provider

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
