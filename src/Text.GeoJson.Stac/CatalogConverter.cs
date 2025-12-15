// -----------------------------------------------------------------------
// <copyright file="CatalogConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The <see cref="Catalog"/> <see cref="JsonConverter"/>.
/// </summary>
internal sealed class CatalogConverter : JsonConverter<Catalog?>
{
    /// <inheritdoc/>
    public override Catalog? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        _ = reader.ReadTo(JsonTokenType.PropertyName);

        string? stacVersion = default;
        List<string?>? stacExtensions = default;
        string? id = default;
        List<Link>? links = default;
        string? title = default;
        string? description = default;

        while (reader.TokenType is not JsonTokenType.EndObject)
        {
            var propertyName = reader.GetString();
            _ = reader.Read();
            if (string.Equals(propertyName, "type", StringComparison.Ordinal))
            {
                if (reader.GetString() is { } typeString)
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                    var type = Enum.Parse<StacType>(typeString);
#else
                    var type = (StacType)Enum.Parse(typeof(StacType), typeString);
#endif
                    if (type is not StacType.Catalog)
                    {
                        throw new InvalidOperationException();
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (string.Equals(propertyName, nameof(id), StringComparison.Ordinal))
            {
                id = reader.GetString();
            }
            else if (string.Equals(propertyName, nameof(title), StringComparison.Ordinal))
            {
                title = reader.GetString();
            }
            else if (string.Equals(propertyName, nameof(description), StringComparison.Ordinal))
            {
                description = reader.GetString();
            }
            else if (string.Equals(propertyName, Constants.StacVersion, StringComparison.Ordinal))
            {
                stacVersion = reader.GetString();
            }
            else if (string.Equals(propertyName, Constants.StacExtensions, StringComparison.Ordinal))
            {
                _ = reader.Read();
                var values = new List<string?>();
                while (reader.TokenType is not JsonTokenType.EndArray)
                {
                    values.Add(reader.GetString());
                    _ = reader.Read();
                }

                stacExtensions = values;
            }
            else if (string.Equals(propertyName, nameof(links), StringComparison.Ordinal))
            {
                links = JsonSerializer.Deserialize<List<Link>>(ref reader, options);
            }
            else
            {
                reader.Skip();
            }

            _ = reader.Read();
        }

        return new()
        {
            Id = ExceptionHelper.ThrowIfNull(id),
#pragma warning disable S3236
            Version = ExceptionHelper.ThrowIfNull(stacVersion, "stac_version"),
#pragma warning restore S3236
            Extensions = stacExtensions,
            Links = ExceptionHelper.ThrowIfNull(links),
            Title = title,
            Description = ExceptionHelper.ThrowIfNull(description),
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Catalog? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteString("type", nameof(StacType.Catalog));
        if (value.Title is { } title)
        {
            writer.WriteString("title", title);
        }

        writer.WriteString(Constants.StacVersion, value.Version);
        if (value.Extensions is { } extensions)
        {
            writer.WritePropertyName(Constants.StacExtensions);
            JsonSerializer.Serialize(writer, extensions, options);
        }

        writer.WriteString("description", value.Description);

        writer.WritePropertyName("links");
        JsonSerializer.Serialize(writer, value.Links, options);

        writer.WriteEndObject();
    }
}