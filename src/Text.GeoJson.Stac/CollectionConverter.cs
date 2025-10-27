// -----------------------------------------------------------------------
// <copyright file="CollectionConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The <see cref="Collection"/> <see cref="JsonConverter"/>.
/// </summary>
internal sealed class CollectionConverter : JsonConverter<Collection?>
{
    private readonly System.Collections.Concurrent.ConcurrentDictionary<JsonSerializerOptions, JsonSerializerOptions> optionsCache = [];

    /// <inheritdoc/>
    public override Collection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
        {
            return null;
        }

        _ = reader.ReadTo(JsonTokenType.PropertyName);

        string? stacVersion = default;
        IReadOnlyList<string?>? stacExtensions = default;
        string? id = default;
        IReadOnlyList<Link>? links = default;
        string? title = default;
        string? description = default;
        IReadOnlyList<string>? keywords = default;
        string? license = default;
        IReadOnlyList<Provider>? providers = default;
        Extent? extent = default;

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
                    if (type is not StacType.Collection)
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
            else if (string.Equals(propertyName, nameof(license), StringComparison.Ordinal))
            {
                license = reader.GetString();
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
            else if (string.Equals(propertyName, nameof(keywords), StringComparison.Ordinal))
            {
                keywords = JsonSerializer.Deserialize<List<string>>(ref reader, options);
            }
            else if (string.Equals(propertyName, nameof(providers), StringComparison.Ordinal))
            {
                providers = JsonSerializer.Deserialize<List<Provider>>(ref reader, options);
            }
            else if (string.Equals(propertyName, nameof(extent), StringComparison.Ordinal))
            {
                extent = JsonSerializer.Deserialize<Extent>(ref reader, options);
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
            Keywords = keywords,
            Providers = providers,
            License = ExceptionHelper.ThrowIfNull(license),
            Extent = ExceptionHelper.ThrowIfNull(extent),
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Collection? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString("id", value.Id);
        writer.WriteString("type", nameof(StacType.Collection));
        if (value.Extensions is { } extensions)
        {
            writer.WritePropertyName(Constants.StacExtensions);
            JsonSerializer.Serialize(writer, extensions, options);
        }

        writer.WriteString(Constants.StacVersion, value.Version);

        writer.WriteString("description", value.Description);
        if (value.Title is { } title)
        {
            writer.WriteString("title", title);
        }

        if (value.Keywords is { } keywords)
        {
            writer.WritePropertyName(nameof(keywords));
            JsonSerializer.Serialize(writer, keywords, options);
        }

        if (value.Providers is { } providers)
        {
            writer.WritePropertyName(nameof(providers));
            JsonSerializer.Serialize(writer, providers, options);
        }

        writer.WritePropertyName("extent");
        JsonSerializer.Serialize(writer, value.Extent, options);

        writer.WriteString("license", value.License);

        writer.WritePropertyName("links");
        JsonSerializer.Serialize(writer, value.Links, options);

        writer.WriteEndObject();
    }
}