// -----------------------------------------------------------------------
// <copyright file="TabReader.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry.MapInfo;

/// <summary>
/// The <c>TAB</c> reader.
/// </summary>
public class TabReader : IDisposable
{
    /// <summary>
    /// The TAB minimum block size.
    /// </summary>
    internal const int TabMinBlockSize = 512;

    /// <summary>
    /// The TAB maximum block size.
    /// </summary>
    internal const int TabMaxBlockSize = short.MaxValue - TabMinBlockSize + 1;

    /// <summary>
    /// The TAB maximum entries block index.
    /// </summary>
    internal const int TabMaxEntriesIndexBlock = (TabMaxBlockSize - 4) / 20;

    private static readonly char[] Separators = [' ', '\t', '(', ')', ',', ';'];

    private readonly Stream stream;

    private readonly bool leaveOpen;

    private bool disposedValue;

    /// <summary>
    /// Initialises a new instance of the <see cref="TabReader"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="TabReader"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public TabReader(Stream stream, bool leaveOpen = false)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(stream);
#else
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }
#endif
        System.Diagnostics.Contracts.Contract.EndContractBlock();
        (this.stream, this.leaveOpen) = (stream, leaveOpen);

        using var streamReader = new StreamReader(this.stream);
        var charset = "Neutral";
        var insideTableDef = false;
        var insideMetadata = false;
        TabField[]? fields = default;
        int currentField = 0;

        while (streamReader.ReadLine() is { } line)
        {
            if (string.Equals(line, "begin_metadata", StringComparison.Ordinal))
            {
                insideTableDef = false;
                insideMetadata = true;
            }
            else if (string.Equals(line, "end_metadata", StringComparison.Ordinal))
            {
                insideMetadata = false;
            }

            var tokenized = line.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

            if (tokenized.Length < 2)
            {
                continue;
            }

            if (string.Equals(tokenized[0], "!version", StringComparison.Ordinal))
            {
                this.Version = int.Parse(tokenized[1], System.Globalization.CultureInfo.InvariantCulture);
                if (this.Version == 100)
                {
                    insideTableDef = true;
                    charset = "Neutral";
                    this.TableType = TabTableType.Native;
                }
            }
            else if (string.Equals(tokenized[0], "!edit_version", StringComparison.Ordinal))
            {
                this.Version = int.Parse(tokenized[1], System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (string.Equals(tokenized[0], "!charset", StringComparison.Ordinal))
            {
                charset = tokenized[1];
            }
            else if (string.Equals(tokenized[0], "Definition", StringComparison.Ordinal) && string.Equals(tokenized[1], "Table", StringComparison.Ordinal))
            {
                insideTableDef = true;
            }
            else if (insideTableDef && fields is null && (string.Equals(tokenized[0], "Type", StringComparison.Ordinal) || string.Equals(tokenized[0], "FORMAT:", StringComparison.Ordinal)))
            {
                this.TableType = tokenized[1] switch
                {
                    "NATIVE" or "LINKED" => TabTableType.Native,
                    "DBF" => TabTableType.DBF,
                    _ => throw new NotSupportedException(),
                };
            }
            else if (insideTableDef && fields is null && string.Equals(tokenized[0], "Description", StringComparison.Ordinal))
            {
                // get the TAB description
                var start =
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    line.IndexOf('\"', StringComparison.Ordinal)
#else
                    line.IndexOf('\"')
#endif
                    + 1;
                var end = line.LastIndexOf('\"');
                this.Description = line[start..end];
            }
            else if (insideTableDef && fields is null && (string.Equals(tokenized[0], "Fields", StringComparison.Ordinal) || string.Equals(tokenized[0], "FIELDS:", StringComparison.Ordinal)))
            {
                var numFields = int.Parse(tokenized[1], System.Globalization.CultureInfo.InvariantCulture);
                if (numFields is < 0 or > 2048)
                {
                    throw new InvalidOperationException();
                }

                fields = new TabField[numFields];
                insideTableDef = false;
            }
            else if (!insideMetadata && fields is not null)
            {
                // reading the fields
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
                var type = Enum.Parse<TabFieldType>(tokenized[1], ignoreCase: true);
#else
                var type = (TabFieldType)Enum.Parse(typeof(TabFieldType), tokenized[1], ignoreCase: true);
#endif
                var width = (type, tokenized) switch
                {
                    (TabFieldType.Date, _) => 10,
                    (TabFieldType.Time, _) => 9,
                    (TabFieldType.DateTime, _) => 19,
                    (TabFieldType.Logical, _) => 1,
                    (_, { Length: > 2 }) => int.Parse(tokenized[2], System.Globalization.CultureInfo.InvariantCulture),
                    _ => default,
                };

                var precision = (type, tokenized) switch
                {
                    (TabFieldType.Decimal, { Length: > 3 }) => int.Parse(tokenized[3], System.Globalization.CultureInfo.InvariantCulture),
                    _ => default,
                };

                fields[currentField++] = new(tokenized[0], type, width, precision);
            }
        }

        this.Fields = fields ?? throw new NotSupportedException("TAB contains no table field definition.  This type of .TAB file cannot be read by this library.");

        this.Encoding = charset switch
        {
            "Neutral" => System.Text.Encoding.Default,
            "WindowsLatin1" => Ensure(System.Text.CodePagesEncodingProvider.Instance.GetEncoding("ISO-8859-1")),
            "WindowsCyrillic" => Ensure(System.Text.CodePagesEncodingProvider.Instance.GetEncoding(1251)),
            _ => throw new InvalidCastException(),
        };

        static System.Text.Encoding Ensure(System.Text.Encoding? encoding)
        {
            return encoding ?? throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Gets the fields.
    /// </summary>
    public IReadOnlyList<TabField> Fields { get; }

    /// <summary>
    /// Gets the table type.
    /// </summary>
    public TabTableType TableType { get; }

    /// <summary>
    /// Gets the description.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public int Version { get; }

    /// <summary>
    /// Gets the charset.
    /// </summary>
    public System.Text.Encoding Encoding { get; }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the resources for this instance.
    /// </summary>
    /// <param name="disposing">Set to <see langword="true"/> to dispose of managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing && !this.leaveOpen)
            {
                this.stream.Dispose();
            }

            this.disposedValue = true;
        }
    }
}