// -----------------------------------------------------------------------
// <copyright file="WktValueKind.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

/// <summary>
/// Specifies the data type of a WKT value.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is correct.")]
public enum WktValueKind
{
    /// <summary>
    /// There is no value.
    /// </summary>
    None,

    /// <summary>
    /// A WKT object.
    /// </summary>
    Object,

    /// <summary>
    /// A WKT string.
    /// </summary>
    String,

    /// <summary>
    /// A WKT number.
    /// </summary>
    Number,

    /// <summary>
    /// A WKT literal.
    /// </summary>
    Literal,
}