#if NET45

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
    /// <summary>
    /// Extension methods for the System.Drawing class
    /// </summary>
    public static class DrawingExtensions
    {
        /// <summary>
        /// Split an Icon (that contains multiple icons) into an array of Icon each rapresenting a single icons.
        /// </summary>
        /// <param name="icon">Instance value.</param>
        /// <returns>An array of <see cref="System.Drawing.Icon"/> objects.</returns>
        public static Icon[] SplitIcon(this Icon icon)
        {
            if (icon == null)
            {
                throw new ArgumentNullException("Can't split the icon. Icon is null.");
            }

            // Get multiple .ico file image.
            byte[] srcBuf = null;
            using (MemoryStream stream = new MemoryStream())
            {
                icon.Save(stream);
                srcBuf = stream.ToArray();
            }

            List<Icon> splitIcons = new List<Icon>();
            {
                const int sICONDIR = 6;            // sizeof(ICONDIR) 
                const int sICONDIRENTRY = 16;      // sizeof(ICONDIRENTRY)

                int count = BitConverter.ToInt16(srcBuf, 4); // ICONDIR.idCount

                for (int i = 0; i < count; i++)
                {
                    using (MemoryStream destStream = new MemoryStream())
                    using (BinaryWriter writer = new BinaryWriter(destStream))
                    {
                        // Copy ICONDIR and ICONDIRENTRY.
                        writer.Write(srcBuf, 0, sICONDIR - 2);
                        writer.Write((short)1);    // ICONDIR.idCount == 1;

                        writer.Write(srcBuf, sICONDIR + sICONDIRENTRY * i, sICONDIRENTRY - 4);
                        writer.Write(sICONDIR + sICONDIRENTRY);    // ICONDIRENTRY.dwImageOffset = sizeof(ICONDIR) + sizeof(ICONDIRENTRY)

                        // Copy picture and mask data.
                        int imgSize = BitConverter.ToInt32(srcBuf, sICONDIR + sICONDIRENTRY * i + 8);       // ICONDIRENTRY.dwBytesInRes
                        int imgOffset = BitConverter.ToInt32(srcBuf, sICONDIR + sICONDIRENTRY * i + 12);    // ICONDIRENTRY.dwImageOffset
                        writer.Write(srcBuf, imgOffset, imgSize);

                        // Create new icon.
                        destStream.Seek(0, SeekOrigin.Begin);
                        splitIcons.Add(new Icon(destStream));
                    }
                }
            }

            return splitIcons.ToArray();
        }

        /// <summary>
        /// Serializes the image in an byte array
        /// </summary>
        /// <param name="image">Instance value.</param>
        /// <param name="format">Specifies the format of the image.</param>
        /// <returns>The image serialized as byte array.</returns>
        public static byte[] ToBytes(this Image image, ImageFormat format)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (format == null)
                throw new ArgumentNullException("format");

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, format);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Gets the bounds of the image in pixels
        /// </summary>
        /// <param name="image">Instance value.</param>
        /// <returns>A rectangle that has the same hight and width as given image.</returns>
        public static Rectangle GetBounds(this Image image)
        {
            return new Rectangle(0, 0, image.Width, image.Height);
        }

        /// <summary>
        /// Gets the rectangle that sorrounds the given point by a specified distance.
        /// </summary>
        /// <param name="p">Instance value.</param>
        /// <param name="distance">Distance that will be used to surround the point.</param>
        /// <returns>Rectangle that sorrounds the given point by a specified distance.</returns>
        public static Rectangle Surround(this Point p, int distance)
        {
            return new Rectangle(p.X - distance, p.Y - distance, distance * 2, distance * 2);
        }

        /// <summary>
        /// 	Scales the bitmap to the passed target size without respecting the aspect.
        /// </summary>
        /// <param name = "bitmap">The source bitmap.</param>
        /// <param name = "size">The target size.</param>
        /// <returns>The scaled bitmap</returns>
        /// <example>
        /// 	<code>
        /// 		var bitmap = new Bitmap("image.png");
        /// 		var thumbnail = bitmap.ScaleToSize(100, 100);
        /// 	</code>
        /// </example>
        public static Bitmap ScaleToSize(this Bitmap bitmap, Size size)
        {
            return bitmap.ScaleToSize(size.Width, size.Height);
        }

        /// <summary>
        /// 	Scales the bitmap to the passed target size without respecting the aspect.
        /// </summary>
        /// <param name = "bitmap">The source bitmap.</param>
        /// <param name = "width">The target width.</param>
        /// <param name = "height">The target height.</param>
        /// <returns>The scaled bitmap</returns>
        /// <example>
        /// 	<code>
        /// 		var bitmap = new Bitmap("image.png");
        /// 		var thumbnail = bitmap.ScaleToSize(100, 100);
        /// 	</code>
        /// </example>
        public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(scaledBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, 0, 0, width, height);
            }
            return scaledBitmap;
        }

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
        /// 	Scales the bitmap to the passed target size by respecting the aspect.
        /// </summary>
        /// <param name = "bitmap">The source bitmap.</param>
        /// <param name = "width">The target width.</param>
        /// <param name = "height">The target height.</param>
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
        public static Bitmap ScaleProportional(this Bitmap bitmap, int width, int height)
        {
            float proportionalWidth, proportionalHeight;

            if (width.Equals(0))
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }
            else if (height.Equals(0))
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else if (((float)width) / bitmap.Size.Width * bitmap.Size.Height <= height)
            {
                proportionalWidth = width;
                proportionalHeight = ((float)width) / bitmap.Size.Width * bitmap.Height;
            }
            else
            {
                proportionalWidth = ((float)height) / bitmap.Size.Height * bitmap.Width;
                proportionalHeight = height;
            }

            return bitmap.ScaleToSize((int)proportionalWidth, (int)proportionalHeight);
        }
        /// <summary>
        ///     A byte[] extension method that converts the @this to an image.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as an Image.</returns>
        public static Image ToImage(this byte[] @this)
        {
            using (var ms = new MemoryStream(@this))
            {
                return Image.FromStream(ms);
            }
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
        /// <param name = "backgroundColor">The color of the background.</param>
        /// <param name = "size">The target size.</param>
        /// <returns>The scaled bitmap</returns>
        /// <example>
        /// 	<code>
        /// 		var bitmap = new Bitmap("image.png");
        /// 		var thumbnail = bitmap.ScaleToSizeProportional(100, 100);
        /// 	</code>
        /// </example>
        public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, Color backgroundColor, Size size)
        {
            return bitmap.ScaleToSizeProportional(backgroundColor, size.Width, size.Height);
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

        /// <summary>
        /// 	Scales the bitmap to the passed target size by respecting the aspect. The overlapping background is filled with the given background color.
        /// </summary>
        /// <param name = "bitmap">The source bitmap.</param>
        /// <param name = "backgroundColor">The color of the background.</param>
        /// <param name = "width">The target width.</param>
        /// <param name = "height">The target height.</param>
        /// <returns>The scaled bitmap</returns>
        /// <example>
        /// 	<code>
        /// 		var bitmap = new Bitmap("image.png");
        /// 		var thumbnail = bitmap.ScaleToSizeProportional(100, 100);
        /// 	</code>
        /// </example>
        public static Bitmap ScaleToSizeProportional(this Bitmap bitmap, Color backgroundColor, int width, int height)
        {
            var scaledBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(scaledBitmap))
            {
                g.Clear(backgroundColor);

                Bitmap proportionalBitmap = bitmap.ScaleProportional(width, height);

                var imagePosition = new Point((int)((width - proportionalBitmap.Width) / 2m),
                                              (int)((height - proportionalBitmap.Height) / 2m));
                g.DrawImage(proportionalBitmap, imagePosition);
            }

            return scaledBitmap;
        }

        /// <summary>
        /// Gets the Image as a Byte[]
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="format">ImageFormat</param>
        /// <returns>A Byte[] of the Image</returns>
        /// <remarks></remarks>
        public static byte[] GetImageInBytes(this Image img, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                if (format != null)
                {
                    img.Save(ms, format);
                    return ms.ToArray();
                }
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets the Image in Base64 format for storage or transfer
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="format">ImageFormat</param>
        /// <returns>Base64 String of the Image</returns>
        /// <remarks></remarks>
        public static string GetImageInBase64(this Image img, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                if (format != null)
                {
                    img.Save(ms, format);
                    return Convert.ToBase64String(ms.ToArray());
                }
                img.Save(ms, img.RawFormat);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// Scales the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="height">The height as int.</param>
        /// <param name="width">The width as int.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Image ScaleImage(this Image img, int height, int width)
        {
            if (img == null || height <= 0 || width <= 0)
            {
                return null;
            }
            int newWidth = (img.Width * height) / (img.Height);
            int newHeight = (img.Height * width) / (img.Width);
            int x, y;

            var bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            // use this when debugging.
            //g.FillRectangle(Brushes.Aqua, 0, 0, bmp.Width - 1, bmp.Height - 1);
            if (newWidth > width)
            {
                // use new height
                x = (bmp.Width - width) / 2;
                y = (bmp.Height - newHeight) / 2;
                g.DrawImage(img, x, y, width, newHeight);
            }
            else
            {
                // use new width
                x = (bmp.Width / 2) - (newWidth / 2);
                y = (bmp.Height / 2) - (height / 2);
                g.DrawImage(img, x, y, newWidth, height);
            }
            // use this when debugging.
            //g.DrawRectangle(new Pen(Color.Red, 1), 0, 0, bmp.Width - 1, bmp.Height - 1);
            return bmp;
        }

        /// <summary>
        /// Resizes the image to fit new size.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="newSize">The new size.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Image ResizeAndFit(this Image image, Size newSize)
        {
            bool sourceIsLandscape = image.Width > image.Height;
            bool targetIsLandscape = newSize.Width > newSize.Height;

            double ratioWidth = newSize.Width / (double)image.Width;
            double ratioHeight = newSize.Height / (double)image.Height;

            double ratio;

            if (ratioWidth > ratioHeight && sourceIsLandscape == targetIsLandscape)
                ratio = ratioWidth;
            else
                ratio = ratioHeight;

            var targetWidth = (int)(image.Width * ratio);
            var targetHeight = (int)(image.Height * ratio);

            var bitmap = new Bitmap(newSize.Width, newSize.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            double offsetX = ((double)(newSize.Width - targetWidth)) / 2;
            double offsetY = ((double)(newSize.Height - targetHeight)) / 2;

            graphics.DrawImage(image, (int)offsetX, (int)offsetY, targetWidth, targetHeight);
            graphics.Dispose();

            return bitmap;
        }

        /// <summary>
        /// Converts to image.
        /// </summary>
        /// <param name="byteArrayIn">The byte array in.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Image ConvertToImage(this byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        ///     An Image extension method that cuts an image.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The cutted image.</returns>
        public static Image Cut(this Image @this, int width, int height, int x, int y)
        {
            var r = new Bitmap(width, height);
            var destinationRectange = new Rectangle(0, 0, width, height);
            var sourceRectangle = new Rectangle(x, y, width, height);

            using (Graphics g = Graphics.FromImage(r))
            {
                g.DrawImage(@this, destinationRectange, sourceRectangle, GraphicsUnit.Pixel);
            }

            return r;
        }
        /// <summary>
        ///     An Image extension method that scales an image to the specific ratio.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="ratio">The ratio.</param>
        /// <returns>The scaled image to the specific ratio.</returns>
        public static Image Scale(this Image @this, double ratio)
        {
            int width = Convert.ToInt32(@this.Width * ratio);
            int height = Convert.ToInt32(@this.Height * ratio);

            var r = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(r))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(@this, 0, 0, width, height);
            }

            return r;
        }

        /// <summary>
        ///     An Image extension method that scales an image to a specific with and height.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>The scaled image to the specific width and height.</returns>
        public static Image Scale(this Image @this, int width, int height)
        {
            var r = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(r))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(@this, 0, 0, width, height);
            }

            return r;
        }

         

        /// <summary>
        /// 生成验证码。
        /// </summary>
        /// <param name="Code">验证码。</param>
        /// <param name="CodeLength">验证码字数。</param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="FontSize"></param>
        /// <returns></returns>
        public static byte[] CreateValidateCode(out string Code, int CodeLength, int Width, int Height, int FontSize)
        {
            String sCode = String.Empty;
            //顏色列表，用於驗證碼、噪線、噪點
            Color[] oColors ={ 
             System.Drawing.Color.Black,
             System.Drawing.Color.Red,
             System.Drawing.Color.Blue,
             System.Drawing.Color.Green,
             System.Drawing.Color.Orange,
             System.Drawing.Color.Brown,
             System.Drawing.Color.Brown,
             System.Drawing.Color.DarkBlue
            };
            //字體列表，用於驗證碼
            string[] oFontNames = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            //驗證碼的字元集，去掉了一些容易混淆的字元
            char[] oCharacter = {
       '2','3','4','5','6','8','9',
       'A','B','C','D','E','F','G','H','J','K', 'L','M','N','P','R','S','T','W','X','Y'
      };
            Random oRnd = new Random();
            Bitmap oBmp = null;
            Graphics oGraphics = null;
            int N1 = 0;
            System.Drawing.Point oPoint1 = default(System.Drawing.Point);
            System.Drawing.Point oPoint2 = default(System.Drawing.Point);
            string sFontName = null;
            Font oFont = null;
            Color oColor = default(Color);

            //生成驗證碼字串
            for (N1 = 0; N1 <= CodeLength - 1; N1++)
            {
                sCode += oCharacter[oRnd.Next(oCharacter.Length)];
            }

            oBmp = new Bitmap(Width, Height);
            oGraphics = Graphics.FromImage(oBmp);
            oGraphics.Clear(System.Drawing.Color.White);
            try
            {
                for (N1 = 0; N1 <= 4; N1++)
                {
                    //畫噪線
                    oPoint1.X = oRnd.Next(Width);
                    oPoint1.Y = oRnd.Next(Height);
                    oPoint2.X = oRnd.Next(Width);
                    oPoint2.Y = oRnd.Next(Height);
                    oColor = oColors[oRnd.Next(oColors.Length)];
                    oGraphics.DrawLine(new Pen(oColor), oPoint1, oPoint2);
                }

                float spaceWith = 0, dotX = 0, dotY = 0;
                if (CodeLength != 0)
                {
                    spaceWith = (Width - FontSize * CodeLength - 10) / CodeLength;
                }

                for (N1 = 0; N1 <= sCode.Length - 1; N1++)
                {
                    //畫驗證碼字串
                    sFontName = oFontNames[oRnd.Next(oFontNames.Length)];
                    oFont = new Font(sFontName, FontSize, FontStyle.Italic);
                    oColor = oColors[oRnd.Next(oColors.Length)];

                    dotY = (Height - oFont.Height) / 2 + 2;//中心下移2像素
                    dotX = Convert.ToSingle(N1) * FontSize + (N1 + 1) * spaceWith;

                    oGraphics.DrawString(sCode[N1].ToString(), oFont, new SolidBrush(oColor), dotX, dotY);
                }

                for (int i = 0; i <= 30; i++)
                {
                    //畫噪點
                    int x = oRnd.Next(oBmp.Width);
                    int y = oRnd.Next(oBmp.Height);
                    Color clr = oColors[oRnd.Next(oColors.Length)];
                    oBmp.SetPixel(x, y, clr);
                }

                Code = sCode;
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                oBmp.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                oGraphics.Dispose();
            }
        }

    }

#endif
