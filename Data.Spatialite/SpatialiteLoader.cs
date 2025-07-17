// -----------------------------------------------------------------------
// <copyright file="SpatialiteLoader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Data.Spatialite;

/// <summary>
/// The spatialite loader.
/// </summary>
internal static class SpatialiteLoader
{
    private const string ExtensionToFind = "mod_spatialite";

    private static string? assetDirectory;

    /// <summary>
    /// Tries to load the mod_spatialite extension into the specified connection.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <returns><see langword="true"/> if the extension was loaded; otherwise, <see langword="false"/>.</returns>
    public static bool TryLoad(Microsoft.Data.Sqlite.SqliteConnection connection)
    {
        try
        {
            Load(connection);

            return true;
        }
        catch (Microsoft.Data.Sqlite.SqliteException ex) when (ex.SqliteErrorCode is 1)
        {
            return false;
        }
    }

    /// <summary>
    /// <para>Loads the mod_spatialite extension into the specified connection.</para>
    /// <para>The the extension will be loaded from native NuGet assets when available.</para>
    /// </summary>
    /// <param name="connection">The connection.</param>
    public static void Load(Microsoft.Data.Sqlite.SqliteConnection connection)
    {
        connection.LoadExtension(FindExtension());

        static string FindExtension()
        {
            if (assetDirectory is not null)
            {
                return ExtensionToFind;
            }

            assetDirectory = Runtime.InteropServices.RuntimeEnvironment.GetRuntimeNativeDirectory(ExtensionToFind + Runtime.InteropServices.RuntimeInformation.SharedLibraryExtension);
            if (assetDirectory is not null)
            {
                Runtime.InteropServices.RuntimeEnvironment.AddDirectoryToPath(assetDirectory);
            }

            return ExtensionToFind;
        }
    }
}