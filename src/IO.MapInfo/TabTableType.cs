// -----------------------------------------------------------------------
// <copyright file="TabTableType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapInfo;

/// <summary>
/// The <c>TAB</c> table type.
/// </summary>
public enum TabTableType
{
    /// <summary>
    /// Native table.
    /// </summary>
    Native,

    /// <summary>
    /// <see cref="Data.Dbf"/> table.
    /// </summary>
    DBF,

    /// <summary>
    /// Access table.
    /// </summary>
    Access,
}