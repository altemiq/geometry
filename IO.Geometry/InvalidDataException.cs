// -----------------------------------------------------------------------
// <copyright file="InvalidDataException.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Geometry;

/// <summary>
/// Represents an exception that may be thrown when input contains invalid data.
/// </summary>
[Serializable]
public class InvalidDataException : Exception
{
    private readonly bool hasMessage;

    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidDataException"/> class.
    /// </summary>
    /// <inheritdoc cref="Exception()"/>
    public InvalidDataException()
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidDataException"/> class.
    /// </summary>
    /// <inheritdoc cref="Exception(string)"/>
    public InvalidDataException(string message)
        : base(message) => this.hasMessage = message is not null;

    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidDataException"/> class.
    /// </summary>
    /// <inheritdoc cref="Exception(string, Exception)"/>
    public InvalidDataException(string message, Exception inner)
        : base(message, inner) => this.hasMessage = message is not null;

#if NETSTANDARD2_0_OR_GREATER || NET20_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    /// <summary>
    /// Initialises a new instance of the <see cref="InvalidDataException"/> class.
    /// </summary>
    /// <inheritdoc cref="Exception(System.Runtime.Serialization.SerializationInfo, System.Runtime.Serialization.StreamingContext)"/>
    protected InvalidDataException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context)
        : base(info, context)
    {
    }
#endif

    /// <summary>
    /// Gets the expected data.
    /// </summary>
    public object? Expected { get; init; }

    /// <summary>
    /// Gets the actual data.
    /// </summary>
    public object? Actual { get; init; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string? Name { get; init; }

    /// <inheritdoc/>
    public override string Message => this.hasMessage ? base.Message : string.Format(Properties.Resources.Culture, Properties.Resources.InvalidData, this.Name, this.Expected, this.Actual);
}