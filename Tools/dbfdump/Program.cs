// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.CommandLine;
using System.Globalization;

var fileArgument = new Argument<FileInfo>("xbase-file").AcceptExistingOnly();
var headerOption = new Option<bool>("-h") { Description = "Write header info (field descriptions)" };
var rawOption = new Option<bool>("-r") { Description = "Write raw field info, numeric values not reformatted" };
var multilineOption = new Option<bool>("-m") { Description = "Multiline, one line per field" };
var command = new RootCommand
{
    fileArgument,
    headerOption,
    rawOption,
    multilineOption,
};

var helpOption = command.Options.OfType<System.CommandLine.Help.HelpOption>().Single();
helpOption.Aliases.Remove("-h");

command.SetAction(async (parseResult, _) =>
{
    var file = parseResult.GetValue(fileArgument)!;
    var header = parseResult.GetValue(headerOption);
    var raw = parseResult.GetValue(rawOption);
    var multiLine = parseResult.GetValue(multilineOption);

    var dbf = Altemiq.IO.Dbf.DbfReader.OpenRead(file.FullName);
    await using (dbf.ConfigureAwait(false))
    {
        if (dbf.Header.FieldCount is 0)
        {
            await parseResult.InvocationConfiguration.Output.WriteLineAsync("There are no fields in this table!").ConfigureAwait(true);
            return;
        }

        if (header)
        {
            for (var i = 0; i < dbf.Header.FieldCount; i++)
            {
                var field = dbf.Header[i];
                var typeName = field.DbfType switch
                {
                    Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Character => "String",
                    Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Number when !field.NumericPrecision.HasValue => "Integer",
                    Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Number => "Double",
                    _ => "Invalid",
                };

                await parseResult.InvocationConfiguration.Output.WriteLineAsync(string.Create(CultureInfo.CurrentCulture, $"Field {i}: Type={field.DbfTypeChar}/{typeName}, Title='{field.ColumnName}', Width={field.ColumnSize ?? 0}, Decimals={field.NumericPrecision ?? 0}")).ConfigureAwait(true);
            }
        }

        // Compute offsets to use when printing each of the field values.
        // We make each field as wide as the field title + 1, or the field value + 1.
        var formats = new (string Name, string Format, string NullFormat, string RawFormat, string Extra)[dbf.Header.FieldCount];

        for (var i = 0; i < dbf.Header.FieldCount; i++)
        {
            var field = dbf.Header[i];
            var titleLength = field.ColumnName.Length;
            var fieldLength = field.ColumnSize ?? 0;
            var fullWidth = Math.Max(titleLength, fieldLength);

            if (!multiLine)
            {
                var headerFormat = field.DbfType == Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Character
                    ? string.Create(CultureInfo.InvariantCulture, $"{{0,-{fullWidth}}} ")
                    : string.Create(CultureInfo.InvariantCulture, $"{{0,{fullWidth}}} ");

                await parseResult.InvocationConfiguration.Output.WriteAsync(string.Format(CultureInfo.CurrentCulture, headerFormat, field.ColumnName)).ConfigureAwait(true);
            }

            var rawFormat = string.Create(CultureInfo.InvariantCulture, $"{{0,-{fieldLength}}}");

            var nullFormat = field.DbfType == Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Character
                ? rawFormat
                : string.Create(CultureInfo.InvariantCulture, $"{{0,{fieldLength}}}");

            var format = field.DbfType switch
            {
                Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Number or Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Float when field.NumericPrecision is { } numericPrecision => string.Create(CultureInfo.InvariantCulture, $"{{0,{fieldLength}:{GetPrecisionString(numericPrecision)}}}"),
                Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Number or Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Float or Altemiq.IO.Dbf.DbfColumn.DbfColumnType.Character => nullFormat,
                _ => throw new InvalidOperationException(),
            };

            var extra = new string(' ', fullWidth - fieldLength + 1);

            formats[i] = (field.ColumnName, format, nullFormat, rawFormat, extra);

            static string GetPrecisionString(int numericPrecision)
            {
                return numericPrecision switch
                {
                    0 => "0.",
                    1 => "0.0",
                    2 => "0.00",
                    3 => "0.000",
                    4 => "0.0000",
                    5 => "0.00000",
                    6 => "0.000000",
                    7 => "0.0000000",
                    8 => "0.00000000",
                    9 => "0.000000000",
                    _ => $"0.{new string('0', numericPrecision)}",
                };
            }
        }

        await parseResult.InvocationConfiguration.Output.WriteLineAsync().ConfigureAwait(true);

        // Read all the records
        for (var j = 0; j < dbf.Header.RecordCount; j++)
        {
            if (multiLine)
            {
                await parseResult.InvocationConfiguration.Output.WriteLineAsync(string.Create(CultureInfo.CurrentCulture, $"Record: {j}")).ConfigureAwait(true);
            }

            if (!dbf.Read(j))
            {
                throw new InvalidOperationException();
            }

            var record = dbf.GetRecord();

            for (var i = 0; i < formats.Length; i++)
            {
                var (name, format, nullFormat, rawFormat, extra) = formats[i];

                if (multiLine)
                {
                    await parseResult.InvocationConfiguration.Output.WriteAsync($"{name}: ").ConfigureAwait(true);
                }

                var text = raw switch
                {
                    false when record.IsDBNull(i) => string.Format(CultureInfo.CurrentCulture, nullFormat, "(NULL)"),
                    false => string.Format(CultureInfo.CurrentCulture, format, record.GetValue(i)),
                    _ => string.Format(CultureInfo.CurrentCulture, rawFormat, record.GetValue(i)),
                };

                await parseResult.InvocationConfiguration.Output.WriteAsync(text).ConfigureAwait(true);

                if (multiLine)
                {
                    await parseResult.InvocationConfiguration.Output.WriteLineAsync().ConfigureAwait(true);
                }
                else
                {
                    // Write out any extra spaces required to pad out the field width.
                    await parseResult.InvocationConfiguration.Output.WriteAsync(extra).ConfigureAwait(true);
                }
            }

            if (record.IsDeleted)
            {
                await parseResult.InvocationConfiguration.Output.WriteAsync("(DELETED)").ConfigureAwait(true);
            }

            await parseResult.InvocationConfiguration.Output.WriteLineAsync().ConfigureAwait(true);
        }
    }
});

await command.Parse(args).InvokeAsync().ConfigureAwait(false);