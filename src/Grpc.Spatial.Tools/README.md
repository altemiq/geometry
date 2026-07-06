# Altemiq.Grpc.Geometry.Tools

A build-time package for gRPC and Protocol Buffer extensions for C# projects. This package contains .proto files and build targets for generating C# code from protobuf definitions.

## Features

- **Build-time only**: Not a runtime dependency
- **Protobuf definitions**: Geometry-related protobuf schemas
- **Code generation**: Generates C# classes from .proto files
- **gRPC support**: Supports gRPC service definitions

## Usage

This package is used at build time to generate C# code from .proto files. Add it as a `DevelopmentDependency` in your project:

```xml
<PackageReference Include="Altemiq.Grpc.Geometry.Tools" Version="*" PrivateAssets="All" />
```

The package includes:
- `altemiq/protobuf/geometry.proto` - Geometry protobuf definitions
- Build targets for automatic code generation

## Target Frameworks

- .NET Standard 1.3
- .NET Framework 4.5

## Dependencies

This is a development dependency package with no runtime dependencies.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/altemiq/geometry/blob/main/LICENSE) file for details.
