// -----------------------------------------------------------------------
// <copyright file="WktSerializer.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy.Serialization;

/// <summary>
/// The <see cref="WellKnownTextNode"/> serializer.
/// </summary>
public static class WktSerializer
{
    private const char StartChar = '[';

    private const char EndChar = ']';

    private const char SeparatorChar = ',';

    /// <summary>
    /// Reads one WKT value from the provided stream into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="stream">The stream containing the WKT.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(Stream stream, WktSerializerOptions? options = default) => Deserialize<TValue>([ReadFromStream(stream)], options);

    /// <summary>
    /// Reads one WKT value from the provided string into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="wkt">The WKT text to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(string wkt, WktSerializerOptions? options = default) => Deserialize<TValue>(wkt.AsSpan(), options);

    /// <summary>
    /// Reads one WKT value from the provided span into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="wkt">The WKT text to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(ReadOnlySpan<char> wkt, WktSerializerOptions? options = default) => Deserialize<TValue>([new WellKnownTextNode(wkt)], options);

    /// <summary>
    /// Reads one WKT value from the provided <see cref="WellKnownTextNode"/> into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="node">The WKT node to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(WellKnownTextNode node, WktSerializerOptions? options = default) => ReadFromNodes<TValue>([node], options);

    /// <summary>
    /// Reads one WKT value from the provided <see cref="WellKnownTextNode"/> instances into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="nodes">The WKT nodes to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(IEnumerable<WellKnownTextNode> nodes, WktSerializerOptions? options = default) => ReadFromNodes<TValue>(nodes, options);

    /// <summary>
    /// Reads one WKT value from the provided <see cref="WellKnownTextNode"/> instances into a <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">The target type of the WKT value.</typeparam>
    /// <param name="nodes">The WKT nodes to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <returns>A <typeparamref name="TValue"/> representation of the WKT value.</returns>
    public static TValue? Deserialize<TValue>(ReadOnlySpan<WellKnownTextNode> nodes, WktSerializerOptions? options = default) => ReadFromNodes<TValue>(nodes, options);

    /// <summary>
    /// Converts the provided value into a <see cref="string"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>A <see cref="string"/> representation of the value.</returns>
    public static string Serialize<TValue>(TValue value, WktSerializerOptions? options = default) => Serialize(WriteToNodes(value, options), options);

    /// <summary>
    /// Converts the provided <see cref="WellKnownTextNode"/> into a <see cref="string"/>.
    /// </summary>
    /// <param name="node">The node to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>A <see cref="string"/> representation of the node.</returns>
    public static string Serialize(WellKnownTextNode node, WktSerializerOptions? options = default)
    {
        return SerializeImpl(node, options ?? WktSerializerOptions.Default, insertNewLine: false, string.Empty);

        static string SerializeImpl(WellKnownTextNode node, WktSerializerOptions options, bool insertNewLine, string indent)
        {
            const string DoubleFormat = "G15";
            const string IndentIncrease = "    ";

            var values = node.Values.Select(value =>
            {
                if (value.TryGetValue(out WellKnownTextNode? node))
                {
                    var newIndent = options.WriteIndented
                        ? IncreaseIndent(indent, IndentIncrease)
                        : string.Empty;
                    return SerializeImpl(node, options, options.WriteIndented, newIndent);
                }

                if (value.TryGetValue(out string? @string))
                {
                    return $"\"{@string}\"";
                }

                if (value.TryGetValue(out double @double))
                {
                    return @double.ToString(DoubleFormat, System.Globalization.CultureInfo.InvariantCulture);
                }

                if (value.TryGetValue(out Literal literal))
                {
                    return literal.ToString();
                }

                throw new System.Diagnostics.UnreachableException();
            });

            var start = insertNewLine ? Environment.NewLine : string.Empty;
            return $"{start}{indent}{node.Id}{StartChar}{string.Join(SeparatorChar.ToString(), values)}{EndChar}";
        }

        static string IncreaseIndent(string current, string increase)
        {
            return string.Concat(current, increase);
        }
    }

    /// <summary>
    /// Converts the provided <see cref="WellKnownTextNode"/> instances into a <see cref="string"/>.
    /// </summary>
    /// <param name="nodes">The nodes to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>A <see cref="string"/> representation of the node.</returns>
    public static string Serialize(IEnumerable<WellKnownTextNode> nodes, WktSerializerOptions? options = default) => string.Join(',', nodes.Select(node => Serialize(node, options)));

    /// <summary>
    /// Gets the node from the value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>A <see cref="WellKnownTextNode"/> representation of the value.</returns>
    /// <exception cref="InvalidOperationException">Failed to find a <see cref="WktConverter"/> for <typeparamref name="TValue"/>.</exception>
    public static WellKnownTextNode GetNode<TValue>(TValue value, WktSerializerOptions? options = default) => GetNodes(value, options).Single();

    /// <summary>
    /// Gets the nodes from the value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>Ths <see cref="WellKnownTextNode"/> representations of the value.</returns>
    /// <exception cref="InvalidOperationException">Failed to find a <see cref="WktConverter"/> for <typeparamref name="TValue"/>.</exception>
    public static IEnumerable<WellKnownTextNode> GetNodes<TValue>(TValue value, WktSerializerOptions? options = default)
    {
        ArgumentNullException.ThrowIfNull(value);
        options ??= WktSerializerOptions.Default;
        var converter = options.GetConverter(typeof(TValue)) ?? throw new InvalidOperationException();
        return converter.WriteAsObject(value, options);
    }

    /// <summary>
    /// Gets the nodes from the values.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>A <see cref="WellKnownTextNode"/> representation of the value.</returns>
    /// <exception cref="InvalidOperationException">Failed to find a <see cref="WktConverter"/> for <paramref name="value"/>.</exception>
    internal static IEnumerable<WellKnownTextNode> GetNodesAsObject(object value, WktSerializerOptions? options = default)
    {
        ArgumentNullException.ThrowIfNull(value);
        options ??= WktSerializerOptions.Default;
        var converter = options.GetConverter(value.GetType()) ?? throw new InvalidOperationException();
        return converter.WriteAsObject(value, options);
    }

    /// <summary>
    /// Unboxes the value.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>The value as <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidCastException">Unable to cast.</exception>
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(value))]
    internal static T? UnboxOnWrite<T>(object? value) => default(T) is not null && value is null ? throw new InvalidCastException($"Unable to cast to {typeof(T)}") : (T?)value;

    private static TValue? ReadFromNodes<TValue>(IEnumerable<WellKnownTextNode> nodes, WktSerializerOptions? options)
    {
        options ??= WktSerializerOptions.Default;
        return options.GetConverter(typeof(TValue)) switch
        {
            WktConverter<TValue> converter => converter.Read(nodes, typeof(TValue), options),
            _ => throw new InvalidOperationException(),
        };
    }

    private static TValue? ReadFromNodes<TValue>(ReadOnlySpan<WellKnownTextNode> nodes, WktSerializerOptions? options)
    {
        options ??= WktSerializerOptions.Default;
        return options.GetConverter(typeof(TValue)) switch
        {
            WktConverter<TValue> converter => converter.Read(nodes, typeof(TValue), options),
            _ => throw new InvalidOperationException(),
        };
    }

    private static IEnumerable<WellKnownTextNode> WriteToNodes<TValue>(TValue value, WktSerializerOptions? options)
    {
        options ??= WktSerializerOptions.Default;
        return options.GetConverter(typeof(TValue)) switch
        {
            WktConverter<TValue> converter => converter.Write(value, options),
            _ => throw new InvalidOperationException(),
        };
    }

    private static WellKnownTextNode ReadFromStream(Stream stream)
    {
        // get the WKT
        using var textReader = new StreamReader(stream);
        var stringBuilder = new System.Text.StringBuilder();
        var count = 0;

        do
        {
            var character = textReader.Read();
            _ = stringBuilder.Append(character);
            if (character == '[')
            {
                count++;
            }
            else if (character == ']')
            {
                count--;
            }
        }
        while (count is not 0);

        return new(stringBuilder.ToString());
    }
}