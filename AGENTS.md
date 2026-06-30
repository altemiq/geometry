# AGENTS.md

## Quick Start

**Target framework**: .NET 10 (global.json:10.0.100, rollForward: latestFeature)

**Build & test**:
```bash
dotnet build
dotnet test
```

**Test framework**: Microsoft.Testing.Platform (xUnit-style, uses TUnit in tests)

## Architecture

**Core library**: `src/Geometry/Geometry.csproj` - target frameworks: net7.0, netstandard2.0

**Sub-projects** (all in `src/`):
- `Data.*` - Data format parsers (DBF, GeoPackage, Spatialite, Sqlite)
- `IO.*` - Input/output formats (Shapefile, MapInfo TAB, GeoJSON, WKT, Spatialite)
- `Grpc.Geometry.Tools` - gRPC/Protobuf tools (build-time dependency, not runtime)
- `Geometry.Protobuf` - Protobuf serialization for geometry types

**Tools** (in `tools/`):
- `dbfdump`, `shpdump`, `shptree`, `shptreevis` - CLI utilities for debugging

## Key Conventions

**Package management**: Centralized via `Directory.Packages.props` (ManagePackageVersionsCentrally=true)

**Code style**: Altemiq.DotNet.CodingStandard (global package reference)

**Polyfill**: Meziantou.Polyfill used extensively to backport .NET APIs to netstandard2.0. Check `MeziantouPolyfill_IncludedPolyfills` in project files.

**Nullable**: Enabled project-wide

**Documentation**: XML doc comments generated (`GenerateDocumentationFile=true`)

**Semantic versioning**: Altemiq.SemanticVersioning.MSBuild (auto-generates version from git)

## CI/CD

**Build workflow**: `.github/workflows/build.yml`
- Tests run on Ubuntu with coverage (TUnit + cobertura)
- NuGet packaging with semantic versioning suffixes: `alpha` (PRs), `beta` (push to main)
- Publishes to nuget.org

**Test workflow**: `.github/workflows/tests.yml` - simpler test-only run on PRs

## Important Notes

**DBF format**: See `docs/xbase/dbf.md` for binary format spec. Key gotchas:
- Version byte at offset 0 determines dBASE/Visual FoxPro variant
- Deleted records marked with `0x2A` (`*`) or space `0x20`
- Header contains binary metadata, records are ASCII (except memo fields)
- dBASE III vs later versions have different field descriptor layouts

**Protobuf**: `src/Grpc.Geometry.Tools/native/include/altemiq/protobuf/geometry.proto` defines wire format. Generated code in `Altemiq.Protobuf.WellKnownTypes` namespace.

**Test projects**: Use `TUnit` framework. Many test projects target net8.0/net9.0/net10.0 only (not netstandard2.0).

**Code page handling**: DBF files may use non-UTF8 encodings. `System.Text.Encoding.CodePages` required for non-UTF8 support (especially on .NET Core).

**Strongly typed resources**: `GenerateStronglyTypedResources=true` in most projects.
