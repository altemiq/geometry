// -----------------------------------------------------------------------
// <copyright file="Provider.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Text.GeoJson.Stac;

/// <summary>
/// The provider.
/// </summary>
public class Provider
{
    /// <summary>
    /// Gets the name of the organization or the individual.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the multi-line description to add further provider information such as processing details for processors and producers, hosting details for hosts or basic contact information.
    /// </summary>
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; init; }

    /// <summary>
    /// Gets the roles of the provider.
    /// </summary>
    [JsonPropertyName("roles")]
    [JsonConverter(typeof(ProviderRolesConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProviderRoles? Roles { get; init; }

    /// <summary>
    /// Gets the homepage on which the provider describes the dataset and publishes contact information.
    /// </summary>
    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Uri? Url { get; init; }

    private sealed class ProviderRolesConverter : JsonConverter<ProviderRoles?>
    {
        public override ProviderRoles? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType is JsonTokenType.StartArray)
            {
                ProviderRoles providerRoles = default;
                reader.Read();
                while (reader.TokenType is not JsonTokenType.EndArray)
                {
                    var value = reader.GetString();
                    if (Enum.TryParse<ProviderRoles>(value, ignoreCase: true, out var providerRole))
                    {
                        providerRoles |= providerRole;
                    }

                    reader.Read();
                }

                return providerRoles;
            }

            throw new InvalidOperationException();
        }

        public override void Write(Utf8JsonWriter writer, ProviderRoles? value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (ProviderRoles providerRole in Enum.GetValues(typeof(ProviderRoles)))
            {
                if (providerRole is not ProviderRoles.None && (value & providerRole) == providerRole)
                {
                    writer.WriteStringValue(providerRole.ToString().ToLowerInvariant());
                }
            }

            writer.WriteEndArray();
        }
    }
}