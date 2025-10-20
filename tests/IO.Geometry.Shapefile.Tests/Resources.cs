// -----------------------------------------------------------------------
// <copyright file="Resources.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

internal static class Resources
{
    public static Stream GetStream(string name) => typeof(Resources).Assembly.GetManifestResourceStream(typeof(Resources), "Data." + name) ?? throw new InvalidOperationException();

    public static byte[] GetBytes(string name)
    {
        var stream = GetStream(name);
        var bytes = new byte[stream.Length];
        _ = stream.Read(bytes, 0, bytes.Length);
        return bytes;
    }
}