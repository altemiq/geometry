# WKT Specification (OGC 18-010r11)

This document provides the WKT (Well-Known Text) specification for coordinate reference systems (CRS) and coordinate operations, optimized for AI agents working with the `Text.Geodesy` library.

## Overview

Section 6 of OGC 18-010r11 defines the WKT string form. Key points:

### Basic Structure
- WKT strings represent CRS or coordinate operation definitions
- Each object is a token: `KEYWORD[attributes...]`
- Nesting allowed to any depth: `KEYWORD1[attr1,KEYWORD2[attr2,attr3]]`
- Keywords are case-insensitive (use uppercase for readability)
- Delimiters: `[`/`]` (preferred) or `(`/`)` - must be consistent throughout string
- Attributes separated by commas
- No whitespace outside quotes (padding allowed for readability, ignored by parsers)
- Text in double quotes; use `""` to escape internal double quotes

### Encoding
- All WKT strings use one encoding throughout
- UTF-8 if no encoding specified
- Characters must be from Unicode repertoire (subset of ISO/IEC 10646)

### Characters
**Basic characters** (ISO/IEC 9075-2:2016):
- Letters: A-Z, a-z
- Digits: 0-9
- Space, double quote (`"`), comma (`,`), period (`.`), hyphen (`-`)
- Special: `+`, `-`, `*`, `/`, `:`, `;`, `<`, `=`, `>`, `?`, `@`, `[`, `]`, `(`, `)`, `{`, `}`, `|`, `^`, `_`, `\`, `&`, `%`, `'`, `#`, `°`

**Numbers**:
- Format: `[sign]integer[.fraction][E[sign]exponent]`
- Decimal separator is period (`.`), not comma
- Examples: `6378137`, `298.257222101`, `1.23E-6`, `-45.5`

**Date and time** (ISO 8601-1 extended format):
- Calendar date: `YYYY[-MM[-DD]]` or `YYYY-DDD`
- Time: `THH[:MM[:SS[.fff]]]` with time zone designator
- Examples: `2014`, `2014-03-01`, `2014-07-12T16:00Z`, `2014-09-18T08:17:56-08`
- Time zone is mandatory

### CRS WKT Characters
**Quoted Latin text** (`&lt;quoted Latin text&gt;`):
- Contains: letters, digits, underscore, space, and special chars: `[](){}<> .=:;+-#%&'*$\?@|°""`
- Use `""` for embedded double quotes: `"Datum origin is 30°25'20""N"`

**Quoted Unicode text** (`&lt;quoted Unicode text&gt;`):
- Any Unicode character except unescaped double quote
- Used for remarks with non-Latin characters (e.g., Japanese, Chinese)

### Reserved Keywords
See section 6.6 for complete list. Key categories:
- CRS types: `GEODCRS`, `GEOGCRS`, `PROJCRS`, `VERTCRS`, `ENGCRS`, `PARAMETRICCRS`, `TIMECRS`
- Base/derived: `BASEGEODCRS`, `BASEGEOGCRS`, `BASEPROJCRS`, `DERIVEDCRS`, etc.
- Components: `ELLIPSOID`, `DATUM`, `TRF`, `GEODETICDATUM`, `PRIMEM`, `PRIMEMERIDIAN`
- Coordinate system: `CS`, `AXIS`, `CSUNIT`, `AXISMINVALUE`, `AXISMAXVALUE`, `RANGEMEANING`
- Units: `LENGTHUNIT`, `ANGLEUNIT`, `SCALEUNIT`, `PARAMETRICUNIT`, `TIMEUNIT`, `TEMPORALQUANTITY`
- Operations: `CONVERSION`, `METHOD`, `PARAMETER`, `COORDINATEOPERATION`, `TRANSFORMATION`
- Metadata: `SCOPE`, `AREA`, `BBOX`, `VERTICALEXTENT`, `TIMEEXTENT`, `ID`, `URI`, `REMARK`

### Backward Compatibility
- See Annex C for mapping from older WKT versions
- See Annex D for changes from ISO 19162:2015
- Implementations should read both old and new syntax forms
- Version detection guidance in Annex B.8

## Key Syntax Rules

1. **Delimiters**: Use consistent delimiter type throughout string (brackets or parentheses)
2. **Case**: Keywords/enumerations are case-insensitive; text content is case-sensitive
3. **Whitespace**: No whitespace outside quotes; padding stripped by parsers
4. **Units**: Must be explicitly specified or implied per CRS type (see 7.4.4 for defaults)
5. **Axis order**: Optional but recommended for clarity; if used, sequence starts at 1

## Text.Geodesy Library Implementation

### Core Types
- **`Utf8WktReader`**: High-performance, forward-only, read-only access to UTF-8 encoded WKT text
- **`Utf8WktWriter`**: High-performance, forward-only, write-only access to UTF-8 encoded WKT text
- **`WktSerializer`**: Static class for serializing/deserializing WKT values
- **`WktObject`**: Internal readonly struct representing a WKT object with ID and values
- **`WktElement`**: Represents a single WKT value (can be Object, String, Number, or Literal)
- **`WktLiteral`**: Ref struct representing a literal value from WKT (enum conversion support)

### Token Types (`WktTokenType`)
- `None`: Unknown token type
- `StartObject`: Start of an object (`[`)
- `EndObject`: End of an object (`]`)
- `Keyword`: A keyword value (e.g., `PROJCRS`)
- `String`: A string value (quoted text)
- `Number`: A number value
- `Literal`: A literal value (unquoted, typically enums)

### Value Kinds (`WktValueKind`)
- `None`: No value
- `Object`: A WKT object
- `String`: A WKT string
- `Number`: A WKT number
- `Literal`: A WKT literal

### Serialization Model
- **`WktConverter<T>`**: Abstract base class for custom converters
  - `Read()`: Convert WKT to type T
  - `Write()`: Convert type T to WKT
- **`WktSerializerOptions`**: Control serialization behavior
- **`WktWriterOptions`**: Control writer behavior (indentation, validation)

### Performance Notes
- All APIs use UTF-8 internally for efficiency
- `Utf8WktReader` and `Utf8WktWriter` use `ReadOnlySpan<byte>` and `Span<byte>` for zero-allocation parsing
- Maximum expansion factor while escaping: 6x (ASCII to escaped sequence)
- Maximum expansion factor while transcoding: 3x (UTF-16 to UTF-8)
- Default buffer growth size: 4096 bytes
- Initial growth size: 256 bytes

### Code Conventions
- Namespace: `Altemiq.Text.Geodesy`
- Internal constants defined in `WktConstants`:
  - `OpenBracket = '['` (byte value)
  - `CloseBracket = ']'` (byte value)
  - `ListSeparator = ','` (byte value)
  - `Quote = '"'` (byte value)
  - `MaximumFormatDoubleLength = 128`
  - `DefaultIndentCharacter = ' '`
  - `DefaultIndentSize = 4`

## References
- OGC 18-010r11: Geographic information — Well-known text representation of coordinate reference systems
- ISO 19111:2019: Geographic information — Referencing by coordinates
- ISO 8601-1: Date and time representations
- ISO/IEC 10646: Universal Coded Character Set
- ISO/IEC 9075-1:2016, 6.2: BNF notation
