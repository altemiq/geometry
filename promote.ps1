$packages = @(
    "Altemiq.Geometry"
    "Altemiq.Geometry.Protobuf"

    "Altemiq.Gcrp.Geometry.Tools"

    "Altemiq.Data.Dbf"
    "Altemiq.Data.Geometry"
    "Altemiq.Data.Sqlite.Core"
    "Altemiq.Data.Sqlite"
    "Altemiq.Data.Spatialite.Core"
    "Altemiq.Data.Spatialite"

    "Altemiq.IO.Geometry"
    "Altemiq.IO.Geometry.Shapefile"
    "Altemiq.IO.Geometry.Spatialite.Core"
    "Altemiq.IO.Geometry.Spatialite"
    
    "Altemiq.Text.GeoJson"
    "Altemiq.Text.GeoJson.Stac")

$packages | ForEach-Object {
    Write-Host "Promoting $_"
    dotnet dnx altemiq.nuget.promote -- source $_ --source https://api.nuget.org/v3/index.json
}