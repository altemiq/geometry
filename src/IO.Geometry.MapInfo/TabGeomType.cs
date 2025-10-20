// -----------------------------------------------------------------------
// <copyright file="TabGeomType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

#pragma warning disable SA1124

/// <summary>
/// The <c>TAB</c> geometry type.
/// </summary>
public enum TabGeomType : byte
{
    /// <summary>
    /// No geometry.
    /// </summary>
    None = 0,

    /// <summary>
    /// Compressed <see cref="Symbol"/>.
    /// </summary>
    SymbolCompressed = 0x01,

    /// <summary>
    /// Symbol.
    /// </summary>
    Symbol = 0x02,

    /// <summary>
    /// Compressed <see cref="Line"/>.
    /// </summary>
    LineCompressed = 0x04,

    /// <summary>
    /// Line.
    /// </summary>
    Line = 0x05,

    /// <summary>
    /// Compressed <see cref="PLine"/>.
    /// </summary>
    PLineCompressed = 0x07,

    /// <summary>
    /// PLine.
    /// </summary>
    PLine = 0x08,

    /// <summary>
    /// Compressed <see cref="Arc"/>.
    /// </summary>
    ArcCompressed = 0x0a,

    /// <summary>
    /// Arc.
    /// </summary>
    Arc = 0x0b,

    /// <summary>
    /// Compressed <see cref="Region"/>.
    /// </summary>
    RegionCompressed = 0x0d,

    /// <summary>
    /// Region.
    /// </summary>
    Region = 0x0e,

    /// <summary>
    /// Compressed <see cref="Text"/>.
    /// </summary>
    TextCompressed = 0x10,

    /// <summary>
    /// Text.
    /// </summary>
    Text = 0x11,

    /// <summary>
    /// Compressed <see cref="Rect"/>.
    /// </summary>
    RectCompressed = 0x13,

    /// <summary>
    /// Rectangle.
    /// </summary>
    Rect = 0x14,

    /// <summary>
    /// Compressed <see cref="RoundRect"/>.
    /// </summary>
    RoundRectCompressed = 0x16,

    /// <summary>
    /// Round rectangle.
    /// </summary>
    RoundRect = 0x17,

    /// <summary>
    /// Compressed <see cref="Ellipse"/>.
    /// </summary>
    EllipseCompressed = 0x19,

    /// <summary>
    /// Ellipse.
    /// </summary>
    Ellipse = 0x1a,

    /// <summary>
    /// Compressed <see cref="MultiPLine"/>.
    /// </summary>
    MultiPLineCompressed = 0x25,

    /// <summary>
    /// Multi-polyline.
    /// </summary>
    MultiPLine = 0x26,

    /// <summary>
    /// Compressed <see cref="FontSymbol"/>.
    /// </summary>
    FontSymbolCompressed = 0x28,

    /// <summary>
    /// Font <see cref="Symbol"/>.
    /// </summary>
    FontSymbol = 0x29,

    /// <summary>
    /// Compressed <see cref="CustomSymbol"/>.
    /// </summary>
    CustomSymbolCompressed = 0x2b,

    /// <summary>
    /// Custom <see cref="Symbol"/>.
    /// </summary>
    CustomSymbol = 0x2c,

    #region Version 450 object types

    /// <summary>
    /// Compressed <see cref="V450Region"/>.
    /// </summary>
    V450RegionCompressed = 0x2e,

    /// <summary>
    /// Region.
    /// </summary>
    V450Region = 0x2f,

    /// <summary>
    /// Compressed <see cref="V450MultiPLine"/>.
    /// </summary>
    V450MultiPLineCompressed = 0x31,

    /// <summary>
    /// Multiple-polyline.
    /// </summary>
    V450MultiPLine = 0x32,

    #endregion

    #region Version 650 object types

    /// <summary>
    /// Compressed <see cref="MultiPoint"/>.
    /// </summary>
    MultiPointCompressed = 0x34,

    /// <summary>
    /// Multi-point.
    /// </summary>
    MultiPoint = 0x35,

    /// <summary>
    /// Compressed <see cref="GeometryCollection"/>.
    /// </summary>
    GeometryCollectionCompressed = 0x37,

    /// <summary>
    /// Geometry collection.
    /// </summary>
    GeometryCollection = 0x38,

    #endregion

    #region Version 800 object types

    /// <summary>
    /// Compressed <see cref="Unknown1"/>.
    /// </summary>
    Unknown1Compressed = 0x3a,  // ???

    /// <summary>
    /// Unknown.
    /// </summary>
    Unknown1 = 0x3b,    // ???

    /// <summary>
    /// Compressed <see cref="V800Region"/>.
    /// </summary>
    V800RegionCompressed = 0x3d,

    /// <summary>
    /// Region.
    /// </summary>
    V800Region = 0x3e,

    /// <summary>
    /// Compressed <see cref="V800MultiPLine"/>.
    /// </summary>
    V800MultiPLineCompressed = 0x40,

    /// <summary>
    /// Multi-polyline.
    /// </summary>
    V800MultiPLine = 0x41,

    /// <summary>
    /// Compressed <see cref="V800MultiPoint"/>.
    /// </summary>
    V800MultiPointCompressed = 0x43,

    /// <summary>
    /// Multi-point.
    /// </summary>
    V800MultiPoint = 0x44,

    /// <summary>
    /// Compressed <see cref="V800Collection"/>.
    /// </summary>
    V800CollectionCompressed = 0x46,

    /// <summary>
    /// Collection.
    /// </summary>
    V800Collection = 0x47,

    #endregion

    /// <summary>
    /// Unset.
    /// </summary>
    Unset = 0xFF,
}

#pragma warning restore SA1124