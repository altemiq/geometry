// -----------------------------------------------------------------------
// <copyright file="DbfWriterOptions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Dbf;

/// <summary>
/// The options for <see cref="DbfWriter"/>.
/// </summary>
public class DbfWriterOptions
{
    /// <summary>
    /// The default options.
    /// </summary>
    public static readonly DbfWriterOptions Default = new();

    /// <summary>
    /// Gets or sets a value indicating whether to allow string truncation.
    /// </summary>
    public bool AllowStringTruncate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow integer truncation.
    /// </summary>
    public bool AllowIntegerTruncate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow decimal truncation.
    /// </summary>
    public bool AllowDecimalTruncate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to write trailing decimals.
    /// </summary>
    public bool WriteTrailingDecimals { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to write null numbers as spaces.
    /// </summary>
    public bool WriteNullNumberAsSpace { get; set; }
}