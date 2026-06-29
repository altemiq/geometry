# Altemiq.Data.Dbf

A .NET library for reading and writing dBASE III and Visual FoxPro DBF files with support for multiple code pages and memo fields.

## Features

- **Full DBF format support**: dBASE III, dBASE IV, Visual FoxPro variants
- **Code page handling**: Support for non-UTF8 encodings via `System.Text.Encoding.CodePages`
- **Memo field support**: Read/write DBT/FPT memo files
- **Deleted record handling**: Properly handles records marked as deleted (`0x2A` or space)
- **High-performance**: Span-based parsing for fast data access

## Usage

```csharp
using Altemiq.Data.Dbf;

// Read a DBF file
using var reader = new DbfReader("data.dbf");
while (reader.Read())
{
    var fieldName = reader.GetString("NAME");
    var value = reader.GetInt32("VALUE");
}

// Write a DBF file
using var writer = new DbfWriter("output.dbf");
writer.Write(new DbfRecord(new[]
{
    new DbfField("NAME", DbfFieldType.Character, 50),
    new DbfField("VALUE", DbfFieldType.Numeric, 10, 0)
}));
```

## Supported DBF Versions

| Version | Byte 0 Value | Description |
|---------|--------------|-------------|
| dBASE III | 0x03 | dBASE III without memo file |
| dBASE IV | 0x04 | dBASE IV without memo file |
| dBASE V | 0x05 | dBASE V without memo file |
| Visual FoxPro | 0x30 | Visual FoxPro (with/without memo/DBC) |

## Target Frameworks

- .NET 7.0
- .NET 6.0
- .NET Standard 2.1
- .NET Standard 2.0
- .NET Framework 4.61

## Dependencies

- [System.Text.Encoding.CodePages](https://www.nuget.org/packages/System.Text.Encoding.CodePages) - For non-UTF8 encoding support
- [System.Buffers](https://www.nuget.org/packages/System.Buffers/) - For netstandard2.0 compatibility
- [System.Memory](https://www.nuget.org/packages/System.Memory/) - For netstandard2.0 compatibility
- [System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple/) - For .NET Framework compatibility

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
