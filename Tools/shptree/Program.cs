// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CommandLine;
using Altemiq.IO.Geometry.Shapefile;

var fileArgument = new Argument<FileInfo>("shpfile") { Description = "the name of the .shp file to index" }.AcceptExistingOnly();

var depthArgument = new Argument<int>("depth") { DefaultValueFactory = _ => 0, Description = "the maximum depth of the index to create, default is 0 meaning that shptree will calculate a reasonable default depth" };

var indexFormatArgument = new Argument<IndexFormat>("index_format") { DefaultValueFactory = _ => BitConverter.IsLittleEndian ? IndexFormat.NL : IndexFormat.NM, Description = "the byte order format for the index" };

var command = new RootCommand
{
    fileArgument,
    depthArgument,
    indexFormatArgument,
};

command.SetAction(parseResult =>
{
    var console = parseResult.InvocationConfiguration.Output;
    var file = parseResult.GetValue(fileArgument)!;
    var depth = parseResult.GetValue(depthArgument);
    var indexFormat = parseResult.GetValue(indexFormatArgument);

    var littleEndian = indexFormat is IndexFormat.NL;
    console.WriteLine($"creating index of new {(littleEndian ? "LSB" : "MSB")}");

    using var reader = new ShapefileReader(file.FullName);
    var node = QixNode.FromShapefile(reader, depth);

    if (node.Nodes.Count is 0 && node.Shapes.Count is 0)
    {
        console.WriteLine("Error generating quadtree");
        return;
    }

    using var writer = new QixWriter(File.OpenWrite(Path.ChangeExtension(file.FullName, "qix")), littleEndian);
    writer.Write(node);
});

return await command
    .Parse(args)
    .InvokeAsync()
    .ConfigureAwait(false);

/// <content>
/// The program class.
/// </content>
internal static partial class Program
{
    /// <summary>
    /// The index format.
    /// </summary>
    internal enum IndexFormat
    {
        /// <summary>
        /// Little endian.
        /// </summary>
        NL = 1,

        /// <summary>
        /// Big endian.
        /// </summary>
        NM = 2,
    }
}