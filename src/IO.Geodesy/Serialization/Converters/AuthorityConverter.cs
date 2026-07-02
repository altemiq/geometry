// -----------------------------------------------------------------------
// <copyright file="AuthorityConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy.Serialization.Converters;

/// <summary>
/// The <see cref="Authority"/> <see cref="Text.Geodesy.Serialization.WktConverter{T}"/>.
/// </summary>
internal sealed class AuthorityConverter : Text.Geodesy.Serialization.WktConverter<Authority>
{
    /// <summary>
    /// The WKT1 converter.
    /// </summary>
    public static readonly AuthorityConverter V1 = new("AUTHORITY");

    /// <summary>
    /// The WKT2 converter.
    /// </summary>
    public static readonly AuthorityConverter V2 = new("ID");

    private readonly string requiredKeyword;

    private AuthorityConverter(string keyword) => this.requiredKeyword = keyword;

    /// <summary>
    /// Tests to see if the specified node is an <see cref="Authority"/> node.
    /// </summary>
    /// <param name="node">The node to test.</param>
    /// <returns><see langword="true"/> if <paramref name="node"/> is a valid <see cref="Authority"/> node; otherwise <see langword="false"/>.</returns>
    public bool IsValidNode(Text.Geodesy.WktElement node) => node.ValueKind is Text.Geodesy.WktValueKind.Object && string.Equals(node.GetKeyword(), this.requiredKeyword, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public override Authority Read(ref Text.Geodesy.Utf8WktReader reader, Type typeToConvert, Text.Geodesy.WktSerializerOptions options)
    {
        if (reader.TokenType is Text.Geodesy.WktTokenType.Keyword)
        {
            // get the keyword
            var keyword = reader.GetString();
            if (!string.Equals(keyword, this.requiredKeyword, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException();
            }

            // move to the object
            if (reader.Read()
                && reader.TokenType is Text.Geodesy.WktTokenType.StartObject
                && reader.Read()
                && reader.TokenType is Text.Geodesy.WktTokenType.String)
            {
                // read the authority
                var name = reader.GetString();

                if (reader.Read())
                {
                    AuthorityCode value = reader.TokenType switch
                    {
                        Text.Geodesy.WktTokenType.String => new(reader.GetString()),
                        Text.Geodesy.WktTokenType.Number => new((int)reader.GetDouble()),
                        _ => throw new InvalidOperationException(),
                    };

                    return new Authority(name, value);
                }
            }
        }

        return default;
    }

    /// <inheritdoc/>
    public override void Write(Text.Geodesy.Utf8WktWriter writer, Authority value, Text.Geodesy.WktSerializerOptions options)
    {
        writer.WriteStartObject(this.requiredKeyword);

        writer.WriteStringValue(value.Name);

        if (value.Value.TryGetValue(out string? s))
        {
            writer.WriteStringValue(s);
        }

        if (value.Value.TryGetValue(out int i))
        {
            writer.WriteNumberValue(i);
        }

        writer.WriteEndObject();
    }
}