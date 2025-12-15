// -----------------------------------------------------------------------
// <copyright file="DbfExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETSTANDARD2_1_OR_GREATER
namespace Altemiq.Data.Dbf;

/// <summary>
/// The <see cref="Dbf"/> extensions.
/// </summary>
internal static class DbfExtensions
{
#pragma warning disable SA1101
    extension(Array)
    {
        /// <summary>
        /// Assigns the given value of type <typeparamref name="T"/> to the elements of the specified array that are within the range of startIndex (inclusive) and the next count number of indices.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The array to be filled.</param>
        /// <param name="value">The new value for the elements in the specified range.</param>
        /// <param name="startIndex">A 32-bit integer that represents the index in <paramref name="array"/> at which filling begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public static void Fill<T>(T[] array, T value, int startIndex, int count)
        {
            for (var i = startIndex; i < startIndex + count; i++)
            {
                array[i] = value;
            }
        }
    }
#pragma warning restore SA1101
}
#endif