// -----------------------------------------------------------------------
// <copyright file="WktTokenType.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

/// <summary>
/// Defines the type of token that was read.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "This is correct.")]
public enum WktTokenType
{
    /// <summary>
    /// An unknown token type.
    /// </summary>
    None,

    /// <summary>
    /// The start of an object.
    /// </summary>
    StartObject,

    /// <summary>
    /// The end of an object.
    /// </summary>
    EndObject,

    /// <summary>
    /// A string value.
    /// </summary>
    String,

    /// <summary>
    /// A number value.
    /// </summary>
    Number,

    /// <summary>
    /// A literal value.
    /// </summary>
    Literal,
}