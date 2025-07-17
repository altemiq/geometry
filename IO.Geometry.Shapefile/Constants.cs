// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.Shapefile;

/// <summary>
/// Constant values.
/// </summary>
public static class Constants
{
    /// <summary>
    /// The SHP extension.
    /// </summary>
    public const string ShpExtension = ".shp";

    /// <summary>
    /// The SHX extension.
    /// </summary>
    public const string ShxExtension = ".shx";

    /// <summary>
    /// The DBF extension.
    /// </summary>
    public const string DbfExtension = ".dbf";

    /// <summary>
    /// The PRJ extension.
    /// </summary>
    public const string PrjExtension = ".prj";

    /// <summary>
    /// The No Data value.
    /// </summary>
    public const double NoData = double.MinValue;

    /// <summary>
    /// The No Data limit.
    /// </summary>
    public const double NoDataLimit = -1E+38;
}