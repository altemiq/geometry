// -----------------------------------------------------------------------
// <copyright file="IRawBlock.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.MapInfo;

/// <summary>
/// Interface for a raw MAP block.
/// </summary>
internal interface IRawBlock
{
    /// <summary>
    /// Gets the offset.
    /// </summary>
    long Offset { get; }

    /// <summary>
    /// Gets the number of bytes used.
    /// </summary>
    int BytesUsed { get; }

    /// <summary>
    /// Determines if this instance contains the offset.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <returns>Returns <see langword="true"/> if this instance contains <paramref name="offset"/>; otherwise <see langword="false"/>.</returns>
    bool ContainsOffset(long offset);
}