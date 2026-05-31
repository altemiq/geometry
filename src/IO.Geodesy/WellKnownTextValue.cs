// -----------------------------------------------------------------------
// <copyright file="WellKnownTextValue.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy;

/// <summary>
/// Represents a well-known text value.
/// </summary>
[System.Runtime.CompilerServices.Union]
public readonly struct WellKnownTextValue : System.Runtime.CompilerServices.IUnion
{
    private readonly WellKnownTextNode? nodeValue;
    private readonly double doubleValue;
    private readonly string? stringValue;
    private readonly Literal literalValue;
    private readonly int tag;

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextValue"/> struct.
    /// </summary>
    /// <param name="node">The node.</param>
    public WellKnownTextValue(WellKnownTextNode node)
    {
        this.nodeValue = node;
        this.tag = this.nodeValue is not null ? 1 : default;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextValue"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public WellKnownTextValue(double value)
    {
        this.doubleValue = value;
        this.tag = 2;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextValue"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public WellKnownTextValue(string value)
    {
        this.stringValue = value;
        this.tag = this.stringValue is not null ? 3 : default;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="WellKnownTextValue"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public WellKnownTextValue(Literal value)
    {
        this.literalValue = value;
        this.tag = 4;
    }

    /// <inheritdoc />
    public object? Value => this.tag switch
    {
        1 => this.nodeValue,
        2 => this.doubleValue,
        3 => this.stringValue!,
        4 => this.literalValue,
        _ => null,
    };

    /// <summary>
    /// Gets a value indicating whether this instance has a value.
    /// </summary>
    public bool HasValue => this.tag is not 0;

    /// <summary>
    /// Tries to get the value.
    /// </summary>
    /// <param name="value">The value to get.</param>
    /// <returns>A value indicating whether <paramref name="value"/> was successfully obtained.</returns>
    public bool TryGetValue([System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out WellKnownTextNode value)
    {
        value = this.nodeValue;
        return this.tag is 1;
    }

    /// <summary>
    /// Tries to get the value.
    /// </summary>
    /// <param name="value">The value to get.</param>
    /// <returns>A value indicating whether <paramref name="value"/> was successfully obtained.</returns>
    public bool TryGetValue(out double value)
    {
        value = this.doubleValue;
        return this.tag is 2;
    }

    /// <summary>
    /// Tries to get the value.
    /// </summary>
    /// <param name="value">The value to get.</param>
    /// <returns>A value indicating whether <paramref name="value"/> was successfully obtained.</returns>
    public bool TryGetValue([System.Diagnostics.CodeAnalysis.MaybeNullWhen(false)] out string value)
    {
        value = this.stringValue;
        return this.tag is 3;
    }

    /// <summary>
    /// Tries to get the value.
    /// </summary>
    /// <param name="value">The value to get.</param>
    /// <returns>A value indicating whether <paramref name="value"/> was successfully obtained.</returns>
    public bool TryGetValue(out Literal value)
    {
        value = this.literalValue;
        return this.tag is 4;
    }
}