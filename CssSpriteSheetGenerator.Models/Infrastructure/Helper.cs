using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;

namespace CssSpriteSheetGenerator.Models
{
    /// <summary>
    ///     <para>Helper methods for <see cref="CssSpriteSheetGenerator.Models" />.</para>
    ///     <para>NOTE: This class is in the Infrastructure subfolder but in the <see cref="CssSpriteSheetGenerator.Models" /> namespace.</para>
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Resizes an array of any dimension.
        /// </summary>
        /// <param name="array">The array to resize.</param>
        /// <param name="newSizes">The new sizes of each dimension of the array.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array" /> cannot be null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="newSizes" /> cannot be null.</exception>
        /// <exception cref="ArgumentException"><paramref name="array" /> must have the same number of dimensions as there are elements in <paramref name="newSizes" />.</exception>
        public static Array ResizeArray(Array array, int[] newSizes)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (newSizes == null)
                throw new ArgumentNullException("newSizes");

            if (newSizes.Length != array.Rank)
                throw new ArgumentException("array must have the same number of dimensions as there are elements in newSizes.", "newSizes");

            var temp = Array.CreateInstance(array.GetType().GetElementType(), newSizes);
            int length = array.Length <= temp.Length ? array.Length : temp.Length;
            Array.ConstrainedCopy(array, 0, temp, 0, length);

            return temp;
        }

        /// <summary>
        /// Sets the color of a bitmap that is in byte array form with pixel format BGRA.
        /// </summary>
        /// <param name="data">The bitmap data.</param>
        /// <param name="pixelIndex">The starting index of the pixel to modify.</param>
        /// <param name="color">The color to change the pixel to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> cannot be null.</exception>
        public static void SetColor(byte[] data, int pixelIndex, Color color)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            checked
            {
                data[pixelIndex + 3] = color.A;
                data[pixelIndex + 2] = color.R;
                data[pixelIndex + 1] = color.G;
                data[pixelIndex + 0] = color.B;
            }
        }

        /// <summary>
        /// Returns the number of bits per pixel for the specified pixel format.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>The number of bits per pixel for the specified pixel format.</returns>
        public static byte GetBitsPerPixel(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    return 1;
                case PixelFormat.Format4bppIndexed:
                    return 4;
                case PixelFormat.Format8bppIndexed:
                    return 8;
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return 16;
                case PixelFormat.Format24bppRgb:
                    return 24;
                case PixelFormat.Canonical:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                case PixelFormat.Format48bppRgb:
                    return 48;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 64;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Returns a randomly generated string of hexadecimal characters with length 13.
        /// </summary>
        /// <returns>The randomly generated string.</returns>
        public static string GenerateRandomString()
        {
            return GenerateRandomString(13);
        }

        /// <summary>
        /// Returns a randomly generated string of hexadecimal characters with length <paramref name="length" />.
        /// </summary>
        /// <param name="length">The length of the string to generate.</param>
        /// <returns>The randomly generated string.</returns>
        public static string GenerateRandomString(int length)
        {
            return Guid.NewGuid().ToString("N").Substring(0, length);
        }

        /// <summary>
        /// Clamps <paramref name="value" /> to the range of <paramref name="min" /> to
        /// <paramref name="max" />.
        /// </summary>
        /// <typeparam name="T">The type of the value to clamp. Must implement <see cref="IComparable{T}" />.</typeparam>
        /// <param name="value">The value to clamp.</param>
        /// <param name="max">The maximum value to clamp to.</param>
        /// <param name="min">The minimum value to clamp to.</param>
        /// <returns>The clamped value.</returns>
        public static T Clamp<T>(this T value, T max, T min) where T : IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="nextItem"></param>
        /// <param name="canContinue"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
                yield return current;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="nextItem"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem) where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        /// <summary>
        /// Gets all exception messages.
        /// </summary>
        /// <param name="exception">The exception to get messages from.</param>
        /// <returns>All of the exception messages from <paramref name="exception" />, each on its own line.</returns>
        public static string GetAllMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            return string.Join(Environment.NewLine, messages);
        }

        /// <summary>
        /// Returns a string formatted independent of culture.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>
        /// A copy of <paramref name="format" /> in which the format items have been 
        /// replaced by the string representation of the corresponding objects in 
        /// <paramref name="args" />.
        /// </returns>
        public static string Invariant(this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}
