#if NET45
using System.Drawing;
using System.Drawing.Drawing2D;
    /// <summary>
    /// 	Extension methods for the System.Drawing.Bitmap class
    /// </summary>
    public static class BitmapExtensions
    {

        /// <summary>
        /// 	Scales the bitmap to the passed target size by respecting the aspect.
        /// </summary>
        /// <param name = "bitmap">The source bitmap.</param>
        /// <param name = "size">The target size.</param>
        /// <returns>The scaled bitmap</returns>
        /// <example>
        /// 	<code>
        /// 		var bitmap = new Bitmap("image.png");
        /// 		var thumbnail = bitmap.ScaleProportional(100, 100);
        /// 	</code>
        /// </example>
        /// <remarks>
        /// 	Please keep in mind that the returned bitmaps size might not match the desired size due to original bitmaps aspect.
        /// </remarks>
        public static Bitmap ScaleProportional(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleProportional(size.Width, size.Height);
        }

        /// <summary>
        /// 	Scales the bitmap to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
        /// </summary>
        /// <param name = "bitmap">The source bitmap.</param>
        /// <param name = "size">The target size.</param>
        /// <returns>The scaled bitmap</returns>
        /// <example>
        /// 	<code>
        /// 		var bitmap = new Bitmap("image.png");
        /// 		var thumbnail = bitmap.ScaleToSizeProportional(100, 100);
        /// 	</code>
        /// </example>
        public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleToSizeProportional(Color.White, size);
        }

        /// <summary>
        /// 	Scales the bitmap to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
        /// </summary>
        /// <param name = "bitmap">The source bitmap.</param>
        /// <param name = "width">The target width.</param>
        /// <param name = "height">The target height.</param>
        /// <returns>The scaled bitmap</returns>
        /// <example>
        /// 	<code>
        /// 		var bitmap = new Bitmap("image.png");
        /// 		var thumbnail = bitmap.ScaleToSizeProportional(100, 100);
        /// 	</code>
        /// </example>
        public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, int width, int height)
        {
            return bitmap.ScaleToSizeProportional(Color.White, width, height);
        }

        
    }
#endif
