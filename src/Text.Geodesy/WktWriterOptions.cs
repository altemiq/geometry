// -----------------------------------------------------------------------
// <copyright file="WktWriterOptions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

/// <summary>
/// The WKT writer options.
/// </summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public struct WktWriterOptions
{
    /// <summary>
    /// The default maximum depth allowed when writing WKT, which is 1000.
    /// </summary>
    internal const int DefaultMaxDepth = 1000;

    private const int OptionsBitCount = 3;
    private const int IndentBit = 1;
    private const int NewLineBit = 2;
    private const int IndentCharacterBit = 4;
    private const int IndentSizeMask = WktConstants.MaximumIndentSize << OptionsBitCount;

    private static readonly string AlternateNewLine = Environment.NewLine.Length is 2 ? WktConstants.NewLineLineFeed : WktConstants.NewLineCarriageReturnLineFeed;

    private int maxDepth;
    private int optionsMask;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="Utf8WktWriter"/> should pretty print the Wkt which includes:
    /// indenting nested Wkt tokens, adding new lines, and adding white space between property names and values.
    /// By default, the Wkt is written without any extra white space.
    /// </summary>
    public bool Indented
    {
        readonly get => (this.optionsMask & IndentBit) is not 0;
        set
        {
            if (value)
            {
                this.optionsMask |= IndentBit;
            }
            else
            {
                this.optionsMask &= ~IndentBit;
            }
        }
    }

    /// <summary>
    /// Gets or sets the indentation character used by <see cref="Utf8WktWriter"/> when <see cref="Indented"/> is enabled. Defaults to the space character.
    /// </summary>
    /// <remarks>Allowed characters are space and horizontal tab.</remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> contains an invalid character.</exception>
    public char IndentCharacter
    {
        readonly get => (this.optionsMask & IndentCharacterBit) is not 0 ? WktConstants.TabIndentCharacter : WktConstants.DefaultIndentCharacter;
        set
        {
            if (value is not WktConstants.DefaultIndentCharacter and not WktConstants.TabIndentCharacter)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (value is not WktConstants.DefaultIndentCharacter)
            {
                this.optionsMask |= IndentCharacterBit;
            }
            else
            {
                this.optionsMask &= ~IndentCharacterBit;
            }
        }
    }

    /// <summary>
    /// Gets or sets the indentation size used by <see cref="Utf8WktWriter"/> when <see cref="Indented"/> is enabled. Defaults to found.
    /// </summary>
    /// <remarks>Allowed values are integers between 0 and 127, included.</remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is out of the allowed range.</exception>
    public int IndentSize
    {
        readonly get => EncodeIndentSize((this.optionsMask & IndentSizeMask) >> OptionsBitCount);
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, WktConstants.MinimumIndentSize);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, WktConstants.MaximumIndentSize);
            this.optionsMask = (this.optionsMask & ~IndentSizeMask) | (EncodeIndentSize(value) << OptionsBitCount);
        }
    }

    /// <summary>
    /// Gets or sets the maximum depth allowed when writing JSON, with the default (i.e. 0) indicating a max depth of 1000.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the max depth is set to a negative value.
    /// </exception>
    public int MaxDepth
    {
        readonly get => this.maxDepth;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
            this.maxDepth = value;
        }
    }

    /// <summary>
    /// Gets or sets the new line string to use when <see cref="Indented"/> is <see langword="true"/>.
    /// The default is the value of <see cref="Environment.NewLine"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the new line string is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the new line string is not <c>\n</c> or <c>\r\n</c>.
    /// </exception>
    public string NewLine
    {
        readonly get => (this.optionsMask & NewLineBit) is not 0 ? AlternateNewLine : Environment.NewLine;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value is not WktConstants.NewLineLineFeed and not WktConstants.NewLineCarriageReturnLineFeed)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (!string.Equals(value, Environment.NewLine, StringComparison.Ordinal))
            {
                this.optionsMask |= NewLineBit;
            }
            else
            {
                this.optionsMask &= ~NewLineBit;
            }
        }
    }

    private static int EncodeIndentSize(int value) => value switch
    {
        0 => WktConstants.DefaultIndentSize,
        WktConstants.DefaultIndentSize => 0,
        _ => value,
    };
}