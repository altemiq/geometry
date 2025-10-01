// -----------------------------------------------------------------------
// <copyright file="TabFieldType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

/// <summary>
/// The types for <see cref="TabField"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "These represent those types")]
public enum TabFieldType
{
    /// <summary>
    /// Unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// <see cref="char"/> type.
    /// </summary>
    Char,

    /// <summary>
    /// <see cref="int"/> type.
    /// </summary>
    Integer,

    /// <summary>
    /// <see cref="short"/> type.
    /// </summary>
    SmallInt,

    /// <summary>
    /// <see cref="long"/> type.
    /// </summary>
    LargeInt,

    /// <summary>
    /// <see cref="double"/> type.
    /// </summary>
    Decimal,

    /// <summary>
    /// <see cref="float"/> type.
    /// </summary>
    Float,

    /// <summary>
    /// <see cref="DateTime"/> type with only date set.
    /// </summary>
    Date,

    /// <summary>
    /// <see cref="bool"/> type.
    /// </summary>
    Logical,

    /// <summary>
    /// <see cref="DateTime"/> type with only time set.
    /// </summary>
    Time,

    /// <summary>
    /// <see cref="System.DateTime"/> type.
    /// </summary>
    DateTime,
}