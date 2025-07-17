// -----------------------------------------------------------------------
// <copyright file="WktSerializerOptions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geodesy.Serialization;

using System.Reflection;

/// <summary>
/// The options for <see cref="WktSerializer"/>.
/// </summary>
public class WktSerializerOptions
{
    private static readonly Dictionary<Type, WktConverter> DefaultSimpleConvertersValue = GetDefaultSimpleConverters();

    private static WktSerializerOptions defaultOptions = new();

    private readonly System.Collections.Concurrent.ConcurrentDictionary<Type, WktConverter?> converters = new();

    /// <summary>
    /// Initialises a new instance of the <see cref="WktSerializerOptions"/> class.
    /// </summary>
    public WktSerializerOptions() => this.Converters = new ConverterList(this);

    /// <summary>
    /// Initialises a new instance of the <see cref="WktSerializerOptions"/> class.
    /// </summary>
    /// <param name="options">The source options.</param>
    public WktSerializerOptions(WktSerializerOptions options)
        : this()
    {
        this.Format = options.Format;
        this.WriteIndented = options.WriteIndented;
        this.IncludeAuthority = options.IncludeAuthority;
        foreach (var converter in options.Converters)
        {
            this.Converters.Add(converter);
        }
    }

    /// <summary>
    /// Gets a read-only, singleton instance of <see cref="WktSerializerOptions"/> that uses the default configuration.
    /// </summary>
    /// <remarks>
    /// Each <see cref="WktSerializerOptions"/> instance encapsulates its own serialization metadata caches, so using fresh default instances every time one is needed can result in redundant recomputation of converters.
    /// This property provides a shared instance that can be consumed by any number of components without necessitating any converter recomputation.
    /// </remarks>
    public static WktSerializerOptions Default
    {
        get
        {
            if (defaultOptions is not { } options)
            {
                options = GetOrCreateDefaultOptionsInstance();
            }

            return options;
        }
    }

    /// <summary>
    /// Gets the list of user-defined converters that were registered.
    /// </summary>
    /// <returns>The list of custom converters.</returns>
    public IList<WktConverter> Converters { get; }

    /// <summary>
    /// Gets a value indicating whether the current instance has been locked for modification.
    /// </summary>
    /// <remarks>
    /// A <see cref="WktSerializerOptions"/> instance can be locked either if
    /// it has been passed to one of the <see cref="WktSerializer"/> methods,
    /// or a user explicitly called the <see cref="MakeReadOnly"/> method on the instance.
    /// </remarks>
    public bool IsReadOnly { get; private set; }

    /// <summary>
    /// Gets or sets the format to use.
    /// </summary>
    public WellKnownTextFormat Format { get; set; } = WellKnownTextFormat.Wkt1;

    /// <summary>
    /// Gets or sets a value indicating whether the WKT should be indented.
    /// </summary>
    public bool WriteIndented { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the WKT should include the authority.
    /// </summary>
    public bool IncludeAuthority { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the WKT should include the axes.
    /// </summary>
    public bool IncludeAxes { get; set; } = true;

    private static IEnumerable<WktConverter> DefaultSimpleConverters
    {
        get
        {
            yield return new Converters.AuthorityConverter();
        }
    }

    /// <summary>
    /// Locks the current instance for further modification.
    /// </summary>
    /// <remarks>This method is idempotent.</remarks>
    public void MakeReadOnly() => this.IsReadOnly = true;

    /// <summary>
    /// Returns the converter for the specified type.
    /// </summary>
    /// <param name="typeToConvert">The type to return a converter for.</param>
    /// <returns>The first converter that supports the given type, or <see langword="null"/> if there is no converter.</returns>
    public WktConverter? GetConverter(Type typeToConvert)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(typeToConvert);
#else
        if (typeToConvert is null)
        {
            throw new ArgumentNullException(nameof(typeToConvert));
        }
#endif
        if (this.converters.TryGetValue(typeToConvert, out var converter))
        {
            return converter;
        }

        converter = this.Converters.FirstOrDefault(c => c.CanConvert(typeToConvert));

        if (converter is null
            && GetUniqueCustomAttribute<WktConverterAttribute>(typeToConvert.GetTypeInfo(), inherit: false) is { } attribute)
        {
            converter = GetConverterFromAttribute(attribute, typeToConvert);
        }

        if (converter is null && DefaultSimpleConvertersValue.TryGetValue(typeToConvert, out var tempConverter))
        {
            converter = tempConverter;
        }

        if (converter is not null && !IsValidConverter(converter, typeToConvert))
        {
            throw new InvalidOperationException();
        }

        if (!this.IsReadOnly)
        {
            _ = this.converters.TryAdd(typeToConvert, converter);
        }

        return converter;

        static bool IsValidConverter(WktConverter converter, Type typeToConvert)
        {
            return converter.TypeToConvert is { } tempType && (tempType.GetTypeInfo().IsAssignableFrom(typeToConvert.GetTypeInfo()) || typeToConvert.GetTypeInfo().IsAssignableFrom(tempType.GetTypeInfo()));
        }

        static TAttribute? GetUniqueCustomAttribute<TAttribute>(MemberInfo memberInfo, bool inherit)
            where TAttribute : Attribute
        {
            using var enumerator = memberInfo
                .GetCustomAttributes<TAttribute>(inherit)
                .GetEnumerator();

            if (!enumerator.MoveNext())
            {
                return default;
            }

            var attribute = enumerator.Current;
            return enumerator.MoveNext()
                ? throw new InvalidOperationException()
                : attribute;
        }

        static WktConverter GetConverterFromAttribute(WktConverterAttribute converterAttribute, Type typeToConvert)
        {
            WktConverter? converter;
            var converterType = converterAttribute.ConverterType;

            if (converterType is null)
            {
                converter = converterAttribute.CreateConverter(typeToConvert) ?? throw new InvalidOperationException();
            }
            else
            {
                var ctor = converterType
#if NETSTANDARD1_1
                    .GetConstructor([]);
#else
                .GetConstructor(Type.EmptyTypes);
#endif
                converter = typeof(WktConverter).GetTypeInfo().IsAssignableFrom(converterType.GetTypeInfo()) && ctor?.IsPublic is true
                    ? Activator.CreateInstance(converterType) as WktConverter
                    : throw new InvalidOperationException();
            }

            return converter?.CanConvert(typeToConvert) is true
                ? converter
                : throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// Verifies that this instance is mutable.
    /// </summary>
    /// <exception cref="InvalidOperationException">Instance is not mutable.</exception>
    internal void VerifyMutable()
    {
        if (this.IsReadOnly)
        {
            throw new InvalidOperationException();
        }
    }

    private static WktSerializerOptions GetOrCreateDefaultOptionsInstance()
    {
        var options = new WktSerializerOptions { IsReadOnly = true };
        return Interlocked.CompareExchange(ref defaultOptions, options, comparand: null) ?? options;
    }

    private static Dictionary<Type, WktConverter> GetDefaultSimpleConverters()
    {
        var dictionary = new Dictionary<Type, WktConverter>(21);
        foreach (var defaultSimpleConverter in DefaultSimpleConverters)
        {
            if (defaultSimpleConverter.TypeToConvert is { } typeToConvert)
            {
                dictionary.Add(typeToConvert, defaultSimpleConverter);
            }
        }

        return dictionary;
    }

    private sealed class ConverterList(WktSerializerOptions options) : IList<WktConverter>
    {
        private readonly List<WktConverter> list = [];

        public int Count => this.list.Count;

        public bool IsReadOnly => false;

        public WktConverter this[int index]
        {
            get => this.list[index];
            set
            {
#if NET6_0_OR_GREATER
                ArgumentNullException.ThrowIfNull(value);
#else
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
#endif
                options.VerifyMutable();
                this.list[index] = value;
            }
        }

        public void Add(WktConverter item)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(item);
#else
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
#endif
            options.VerifyMutable();
            this.list.Add(item);
        }

        public void Clear()
        {
            options.VerifyMutable();
            this.list.Clear();
        }

        public bool Contains(WktConverter item) => this.list.Contains(item);

        public void CopyTo(WktConverter[] array, int arrayIndex) => this.list.CopyTo(array, arrayIndex);

        public IEnumerator<WktConverter> GetEnumerator() => this.list.GetEnumerator();

        public int IndexOf(WktConverter item) => this.list.IndexOf(item);

        public void Insert(int index, WktConverter item)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(item);
#else
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
#endif
            options.VerifyMutable();
            this.list.Insert(index, item);
        }

        public bool Remove(WktConverter item)
        {
            options.VerifyMutable();
            return this.list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            options.VerifyMutable();
            this.list.RemoveAt(index);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.list.GetEnumerator();
    }
}