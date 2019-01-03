using System;
using System.Drawing;
/// <summary>
/// Color Extensions
/// </summary>
    public static class ColorExtensions
    {
#if NET45
     /// <summary>
    /// returns the RGB Value of a color.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <returns>string</returns>
    /// <remarks></remarks>
    public static string ToHtmlColor(this Color color)
        {
            return ColorTranslator.ToHtml(color);
        }
        public static Color FromHtmlColor(this string strColor)
        {
            return ColorTranslator.FromHtml(strColor);
        }
        /// <summary>
        ///     Translates an OLE color value to a GDI+  structure.
        /// </summary>
        /// <param name="oleColor">The OLE color to translate.</param>
        /// <returns>The  structure that represents the translated OLE color.</returns>
        public static Color FromOle(this Int32 oleColor)
        {
            return ColorTranslator.FromOle(oleColor);
        }
        /// <summary>
        /// returns the OLE Value of the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int ToOleColor(this Color color)
        {
            return ColorTranslator.ToOle(color);
        }
        /// <summary>
        ///     Translates a Windows color value to a GDI+  structure.
        /// </summary>
        /// <param name="win32Color">The Windows color to translate.</param>
        /// <returns>The  structure that represents the translated Windows color.</returns>
        public static Color FromWin32(this Int32 win32Color)
        {
            return ColorTranslator.FromWin32(win32Color);
        }

        /// <summary>
        /// returns the Win32 value of the color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int ToWin32Color(this Color color)
        {
            return ColorTranslator.ToWin32(color);
        }
#endif


    /// <summary>
    ///     Creates a  structure from a 32-bit ARGB value.
    /// </summary>
    /// <param name="argb">A value specifying the 32-bit ARGB value.</param>
    /// <returns>The  structure that this method creates.</returns>
    public static Color FromArgb(this Int32 argb)
        {
            return Color.FromArgb(argb);
        }

        /// <summary>
        ///     Creates a  structure from the four ARGB component (alpha, red, green, and blue) values. Although this method
        ///     allows a 32-bit value to be passed for each component, the value of each component is limited to 8 bits.
        /// </summary>
        /// <param name="argb">A value specifying the 32-bit ARGB value.</param>
        /// <param name="red">The red component. Valid values are 0 through 255.</param>
        /// <param name="green">The green component. Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component. Valid values are 0 through 255.</param>
        /// <returns>The  that this method creates.</returns>
        /// ###
        /// <param name="alpha">The alpha component. Valid values are 0 through 255.</param>
        public static Color FromArgb(this Int32 argb, Int32 red, Int32 green, Int32 blue)
        {
            return Color.FromArgb(argb, red, green, blue);
        }

        /// <summary>
        ///     Creates a  structure from the specified  structure, but with the new specified alpha value. Although this
        ///     method allows a 32-bit value to be passed for the alpha value, the value is limited to 8 bits.
        /// </summary>
        /// <param name="argb">A value specifying the 32-bit ARGB value.</param>
        /// <param name="baseColor">The  from which to create the new .</param>
        /// <returns>The  that this method creates.</returns>
        /// ###
        /// <param name="alpha">The alpha value for the new . Valid values are 0 through 255.</param>
        public static Color FromArgb(this Int32 argb, Color baseColor)
        {
            return Color.FromArgb(argb, baseColor);
        }

        /// <summary>
        ///     Creates a  structure from the specified 8-bit color values (red, green, and blue). The alpha value is
        ///     implicitly 255 (fully opaque). Although this method allows a 32-bit value to be passed for each color
        ///     component, the value of each component is limited to 8 bits.
        /// </summary>
        /// <param name="argb">A value specifying the 32-bit ARGB value.</param>
        /// <param name="green">The green component value for the new . Valid values are 0 through 255.</param>
        /// <param name="blue">The blue component value for the new . Valid values are 0 through 255.</param>
        /// <returns>The  that this method creates.</returns>
        /// ###
        /// <param name="red">The red component value for the new . Valid values are 0 through 255.</param>
        public static Color FromArgb(this Int32 argb, Int32 green, Int32 blue)
        {
            return Color.FromArgb(argb, green, blue);
        }


        public static Color? ToColor(this string str, bool htmlSafe = false)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            str = str.Trim();
            if (str.StartsWith("#"))
            {
                str = str.Substring(1);
            }
            else if (str.StartsWith("rgb("))
            {
                var parts = str.Substring("rgb(".Length).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3)
                    return null;
                int r = parts[0].ToInt32();
                int g = parts[1].ToInt32();
                int b = parts[2].ToInt32();

                if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
                    return null;

                return Color.FromArgb(r, g, b);
            }
            else if (str.StartsWith("rgba("))
            {
                var parts = str.Substring("rgba(".Length).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 4)
                    return null;
                int r = parts[0].ToInt32();
                int g = parts[1].ToInt32();
                int b = parts[2].ToInt32();
                int a = (int)(255 * parts[3].ToDouble());

                if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255 || a < 0 || a > 255)
                    return null;

                return Color.FromArgb(a, r, g, b);

            }
            else if (str.StartsWith("hsl("))
            {
                var parts = str.Substring("hsl(".Length).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3)
                    return null;
                double h = parts[0].ToDouble() / 360.0;
                double s = parts[1].ToDouble();
                double l = parts[2].ToDouble();

                // h can be any value actually, since it gets converted to 0-360 (wheel of color)
                if (s < 0 || s > 1 || l < 0 || l > 1)
                    return null;

                return ColorExtensions.FromHsl(h, s, l);
            }
            else if (str.StartsWith("hsla("))
            {
                var parts = str.Substring("hsla(".Length).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 4)
                    return null;
                double h = parts[0].ToDouble() / 360.0;
                double s = parts[1].ToDouble();
                double l = parts[2].ToDouble();
                double a = parts[3].ToDouble();

                if (s < 0 || s > 1 || l < 0 || l > 1 || a < 0 || a > 1)
                    return null;

                return ColorExtensions.FromAhsl(a, h, s, l);

            }
            else
            {
                try
                {
                    var color = Color.FromName(str);
                    if (!(color.R == 0 && color.G == 0 && color.B == 0 && color.A == 0) || string.Compare(str, "black", true) == 0)
                        return color;
                }
                catch (Exception)
                {
                }
                if (htmlSafe)
                    return null; // must be either a named color or start with #/rgb to be HTML safe
            }


            if (str.Length == 3)
            {
                str = string.Concat(str[0], str[0], str[1], str[1], str[2], str[2]);
            }
            else if (str.Length != 6)
            {
                return null;
            }

            byte[] bytes = str.HexStringToBytes();

            if (bytes == null || bytes.Length != 3)
                return null;

            return Color.FromArgb(bytes[0], bytes[1], bytes[2]);

        }

        public static Color FromHsl(double hue, double saturation, double luminosity)
        {
            return FromAhsl(1.0, hue, saturation, luminosity);
        }

        public static Color FromAhsl(double alpha, double hue, double saturation, double luminosity)
        {
            return FromHsl(Hsl.FromAhsl(alpha, hue, saturation, luminosity));
        }
        public static Color FromHsl(Hsl hsl)
        {
            Func<double, double, double, double> hueToRgb = (c, t1, t2) =>
            {
                if (c < 0) c += 1.0;
                if (c > 1) c -= 1.0;
                if (6.0 * c < 1.0) return t1 + (t2 - t1) * 6.0 * c;
                if (2.0 * c < 1.0) return t2;
                if (3.0 * c < 2.0) return t1 + (t2 - t1) * (2.0 / 3.0 - c) * 6.0;
                return t1;
            };

            int alpha = (int)Math.Round(hsl.Alpha * 255.0);
            if (Math.Abs(hsl.Saturation) < Hsl.MaxHslColorPrecision)
            {
                var mono = (int)Math.Round(hsl.Luminosity * 255.0);
                return Color.FromArgb(alpha, mono, mono, mono);
            }
            else
            {
                double t2 = hsl.Luminosity < 0.5
                    ? hsl.Luminosity * (1.0 + hsl.Saturation)
                    : (hsl.Luminosity + hsl.Saturation) - (hsl.Luminosity * hsl.Saturation);
                double t1 = 2.0 * hsl.Luminosity - t2;

                var r = (int)Math.Round(hueToRgb(hsl.Hue + 1.0 / 3.0, t1, t2) * 255.0);
                var g = (int)Math.Round(hueToRgb(hsl.Hue, t1, t2) * 255.0);
                var b = (int)Math.Round(hueToRgb(hsl.Hue - 1.0 / 3.0, t1, t2) * 255.0);

                return Color.FromArgb(alpha, r, g, b);
            }
        }

        public static Hsl ToHsl(this Color color)
        {
            double r = (color.R / 255.0);
            double g = (color.G / 255.0);
            double b = (color.B / 255.0);

            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);
            double delta = max - min;

            Hsl hsl = new Hsl();
            hsl.Alpha = color.A / 255.0;
            hsl.Luminosity = ((max + min) / 2.0);

            if (Math.Abs(delta) > Hsl.MaxHslColorPrecision)
            {
                if (hsl.Luminosity < 0.5)
                {
                    hsl.Saturation = (delta / (max + min));
                }
                else
                {
                    hsl.Saturation = (delta / (2.0 - max - min));
                }

                if (Math.Abs(r - max) < Hsl.MaxHslColorPrecision)
                {
                    hsl.Hue = (g - b) / delta + (g < b ? 6 : 0);
                }
                else if (Math.Abs(g - max) < Hsl.MaxHslColorPrecision)
                {
                    hsl.Hue = 2 + (b - r) / delta;
                }
                else
                {
                    hsl.Hue = 4 + (r - g) / delta;
                }
                hsl.Hue = hsl.Hue / 6.0;
            }

            return hsl;
        }

        public static string ToHexColor(this Color color)
        {
            return color.R.ToString("x2") + color.G.ToString("x2") + color.B.ToString("x2");
        }

        

    }

    public class Hsl
    {
        public static readonly double MaxHslColorPrecision = 0.003;

        public double Alpha { get; set; }
        public double Hue { get; set; }
        public double Saturation { get; set; }
        public double Luminosity { get; set; }

        public Hsl()
        {
            Alpha = 1.0;
        }

        public static Hsl FromHsl(double hue, double saturation, double luminosity)
        {
            var hsl = new Hsl
            {
                Hue = hue,
                Saturation = saturation,
                Luminosity = luminosity
            };

            return hsl;
        }

        public static Hsl FromAhsl(double alpha, double hue, double saturation, double luminosity)
        {
            var hsl = new Hsl
            {
                Alpha = alpha,
                Hue = hue,
                Saturation = saturation,
                Luminosity = luminosity
            };

            return hsl;
        }

        public override bool Equals(object obj)
        {
            var rhs = obj as Hsl;
            if (rhs == null)
                return false;


            return Math.Abs(Hue - rhs.Hue) < MaxHslColorPrecision
                   && Math.Abs(Luminosity - rhs.Luminosity) < MaxHslColorPrecision
                   && Math.Abs(Saturation - rhs.Saturation) < MaxHslColorPrecision
                   && Math.Abs(Alpha - rhs.Alpha) < MaxHslColorPrecision;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "HSL[A=" + Alpha.ToString("N4")
                   + ", H=" + Hue.ToString("N4")
                   + ", S=" + Saturation.ToString("N4")
                   + ", L=" + Luminosity.ToString("N4")
                   + "]";
        }
    }