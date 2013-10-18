using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CssSpriteSheetGenerator.Gui.Converters
{
    /// <summary>
    /// Represents the converter that converts <see cref="Bitmap" /> to and from <see cref="ImageBrush" />. 
    /// </summary>
    public class BitmapToImageBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="Bitmap" /> to an <see cref="ImageBrush" />.
        /// </summary>
        /// <param name="value">The <see cref="Bitmap" /> to convert.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>An equivalent <see cref="ImageBrush" /> instance.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            using (var memoryStream = new MemoryStream())
            {
                var bitmap = value as Bitmap;
                if (bitmap == null)
                    return new ImageBrush();

                bitmap.Save(memoryStream, ImageFormat.Png);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                var imageBrush = new ImageBrush(bitmapImage);

                return imageBrush;
            }
        }

        /// <summary>
        /// Converting from a <see cref="ImageBrush" /> to a <see cref="Bitmap" /> is not supported.
        /// </summary>
        /// <param name="value">This parameter is not used.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>An equivalent <see cref="Bitmap" /> instance.</returns>
        /// <exception cref="NotSupportedException">Converting from a <see cref="ImageBrush" /> to a <see cref="Bitmap" /> is not supported.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ImageBrush")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(string.Format(culture, "Converting from an ImageBrush to a Bitmap is not supported."));
        }
    }
}
