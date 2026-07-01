// -----------------------------------------------------------------------
// <copyright file="WktConstants.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.Geodesy;

#pragma warning disable SA1600

internal static class WktConstants
{
    public const byte OpenBracket = (byte)'[';
    public const byte CloseBracket = (byte)']';
    public const byte CarriageReturn = (byte)'\r';
    public const byte LineFeed = (byte)'\n';
    public const byte Tab = (byte)'\t';
    public const byte ListSeparator = (byte)',';
    public const byte Quote = (byte)'"';

    public const int RemoveFlagsBitMask = 0x7FFFFFFF;

    // In the worst case, an ASCII character represented as a single utf-8 byte could expand 6x when escaped.
    // For example: '+' becomes '\u002B'
    // Escaping surrogate pairs (represented by 3 or 4 utf-8 bytes) would expand to 12 bytes (which is still <= 6x).
    // The same factor applies to utf-16 characters.
    // This factor also serves as an upper bound for the combined escaping-and-transcoding pipeline.
    // A non-ASCII unicode character is either:
    // - escaped into an ASCII sequence (e.g. \uXXXX), so 1 UTF-16 char -> at most 6 UTF-8 bytes, or
    // - written directly as UTF-8 (e.g. when using a non-default encoder such as UnsafeRelaxedJsonEscaping),
    //   expanding at most 3x (MaxExpansionFactorWhileTranscoding), which is <= 6.
    public const int MaxExpansionFactorWhileEscaping = 6;

    // In the worst case, a single UTF-16 character could be expanded to 3 UTF-8 bytes.
    // Only surrogate pairs expand to 4 UTF-8 bytes but that is a transformation of 2 UTF-16 characters going to 4 UTF-8 bytes (factor of 2).
    // All other UTF-16 characters can be represented by either 1 or 2 UTF-8 bytes.
    public const int MaxExpansionFactorWhileTranscoding = 3;

    public const string NewLineLineFeed = "\n";
    public const string NewLineCarriageReturnLineFeed = "\r\n";

    public const int MaximumFormatDoubleLength = 128;  // default (i.e. 'G'), using 128 (rather than say 32) to be future-proof.

    public const char DefaultIndentCharacter = ' ';
    public const char TabIndentCharacter = '\t';
    public const int DefaultIndentSize = 4;
    public const int MinimumIndentSize = 0;
    public const int MaximumIndentSize = 127;
}