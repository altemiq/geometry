// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CommandLine;
using Altemiq.IO.Geometry.Shapefile;

var shapefileArgument = new Argument<FileInfo>("shapefile") { Description = "the name of the .shp file to export" }.AcceptExistingOnly();
var newShapefileArgument = new Argument<FileInfo>("new_shapefile") { Description = "the name of the .shp file to save to" };

var command = new RootCommand
{
    shapefileArgument,
    newShapefileArgument,
};

command.SetAction(async (parseResult, cancellationToken) =>
{
    var console = parseResult.InvocationConfiguration.Output;
    var shapefile = parseResult.GetValue(shapefileArgument)!;
    var newShapefile = parseResult.GetValue(newShapefileArgument)!;

    var stream = File.OpenRead(Path.ChangeExtension(shapefile.FullName, ".qix"));
    await using (stream.ConfigureAwait(true))
    {
        var treeReader = new QixReader(stream);

        var writer = new ShapefileWriter(
            Path.ChangeExtension(newShapefile.FullName, ".shp"),
            ShpType.Polygon,
            new Altemiq.IO.Dbf.DbfColumn("ITEMS", Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Number, 15),
            new Altemiq.IO.Dbf.DbfColumn("SUBNODES", Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Number, 15),
            new Altemiq.IO.Dbf.DbfColumn("FACTOR", Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Number, 15));

        await console.WriteLineAsync(string.Create(System.Globalization.CultureInfo.CurrentCulture, $"This new {ByteOrder(treeReader.Header.IsLittleEndian)} index supports a shapefile with {treeReader.Header.Count} shapes, {treeReader.Header.Depth} depth ")).ConfigureAwait(true);

        while (treeReader.Read() is var node && !cancellationToken.IsCancellationRequested)
        {
            var shapeCount = node.Shapes.Count;
            var nodeCount = node.Nodes.Count;

            var polygon = new Altemiq.Geometry.Polygon(GetPoints(node.Extents));

            writer.Write(polygon, shapeCount, nodeCount, null);

            static IEnumerable<Altemiq.Geometry.Point> GetPoints(Altemiq.Geometry.Envelope envelope)
            {
                yield return new(envelope.Left, envelope.Bottom);
                yield return new(envelope.Right, envelope.Bottom);
                yield return new(envelope.Right, envelope.Top);
                yield return new(envelope.Left, envelope.Top);
                yield return new(envelope.Left, envelope.Bottom);
            }
        }
    }

    static string ByteOrder(bool littleEndian)
    {
        return littleEndian ? "LSB" : "MSB";
    }
});

return await command
    .Parse(args)
    .InvokeAsync()
    .ConfigureAwait(false);