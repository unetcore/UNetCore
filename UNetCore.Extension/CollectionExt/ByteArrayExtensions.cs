using System;
using System.IO;
using System.Text;
/// <summary>
/// 	Extension methods for byte-Arrays
/// </summary>
public static class ByteArrayExtensions
{
    internal static string Base62Index = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private static string[] _byteMap;
    private static int[] _toByteMap;

    static ByteArrayExtensions()
    {
        _byteMap = new string[256];
        for (int i = 0; i < 256; i++)
            _byteMap[i] = i.ToString("x2");

        _toByteMap = new int[(int)'g'];
        for (int i = 0; i < _toByteMap.Length; i++)
            _toByteMap[i] = -1;

        for (char c = '0'; c <= '9'; c++)
        {
            _toByteMap[c] = (byte)(c - '0');
        }
        for (char c = 'a'; c <= 'f'; c++)
        {
            _toByteMap[c] = (byte)(10 + c - 'a');
            var c2 = char.ToUpper(c);
            _toByteMap[c2] = (byte)(10 + c2 - 'A');
        }
    }
    public static byte[] FromHex(this string hexString)
    {
        if ((hexString == null) || ((hexString.Length % 2) != 0))
        {
            return null;
        }
        byte[] buffer = new byte[hexString.Length / 2];
        for (int i = 0; i < buffer.Length; i++)
        {
            int num2 = HexToInt(hexString[2 * i]);
            int num3 = HexToInt(hexString[(2 * i) + 1]);
            if ((num2 == -1) || (num3 == -1))
            {
                return null;
            }
            buffer[i] = (byte)((num2 << 4) | num3);
        }
        return buffer;
    }

    private static int HexToInt(char h)
    {
        if ((h >= '0') && (h <= '9'))
        {
            return (h - '0');
        }
        if ((h >= 'a') && (h <= 'f'))
        {
            return ((h - 'a') + 10);
        }
        if ((h >= 'A') && (h <= 'F'))
        {
            return ((h - 'A') + 10);
        }
        return -1;
    }

    private static char NibbleToHex(byte nibble)
    {
        if (nibble >= 10)
        {
            return (char)((nibble - 10) + 0x41);
        }
        return (char)(nibble + 0x30);
    }

    public static string ToHex(this byte[] bytes)
    {
        if (bytes == null)
        {
            return null;
        }
        char[] chArray = new char[bytes.Length * 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            byte num2 = bytes[i];
            chArray[2 * i] = NibbleToHex((byte)(num2 >> 4));
            chArray[(2 * i) + 1] = NibbleToHex((byte)(num2 & 15));
        }
        return new string(chArray);
    }
    /// <summary>
    /// 	Find the first occurence of an byte[] in another byte[]
    /// </summary>
    /// <param name = "buf1">the byte[] to search in</param>
    /// <param name = "buf2">the byte[] to find</param>
    /// <returns>the first position of the found byte[] or -1 if not found</returns>
    /// <remarks>
    /// </remarks>
    public static int FindArrayInArray(this byte[] buf1, byte[] buf2)
    {
        if (buf2 == null)
            throw new ArgumentNullException("buf2");

        if (buf1 == null)
            throw new ArgumentNullException("buf1");

        if (buf2.Length == 0)
            return 0;       // by definition empty sets match immediately

        int j = -1;
        int end = buf1.Length - buf2.Length;
        while ((j = Array.IndexOf(buf1, buf2[0], j + 1)) <= end && j != -1)
        {
            int i = 1;
            while (buf1[j + i] == buf2[i])
            {
                if (++i == buf2.Length)
                    return j;
            }
        }
        return -1;
    }


    #region Z.EXT
    /// <summary>
    ///     Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with
    ///     base-64 digits.
    /// </summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <returns>The string representation, in base 64, of the contents of .</returns>
    public static String ToBase64String(this Byte[] inArray)
    {
        return Convert.ToBase64String(inArray);
    }

    /// <summary>
    ///     Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with
    ///     base-64 digits. A parameter specifies whether to insert line breaks in the return value.
    /// </summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <param name="options">to insert a line break every 76 characters, or  to not insert line breaks.</param>
    /// <returns>The string representation in base 64 of the elements in .</returns>
    public static String ToBase64String(this Byte[] inArray, Base64FormattingOptions options)
    {
        return Convert.ToBase64String(inArray, options);
    }

    /// <summary>
    ///     Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation that is
    ///     encoded with base-64 digits. Parameters specify the subset as an offset in the input array, and the number of
    ///     elements in the array to convert.
    /// </summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <param name="offset">An offset in .</param>
    /// <param name="length">The number of elements of  to convert.</param>
    /// <returns>The string representation in base 64 of  elements of , starting at position .</returns>
    public static String ToBase64String(this Byte[] inArray, Int32 offset, Int32 length)
    {
        return Convert.ToBase64String(inArray, offset, length);
    }

    /// <summary>
    ///     Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation that is
    ///     encoded with base-64 digits. Parameters specify the subset as an offset in the input array, the number of
    ///     elements in the array to convert, and whether to insert line breaks in the return value.
    /// </summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <param name="offset">An offset in .</param>
    /// <param name="length">The number of elements of  to convert.</param>
    /// <param name="options">to insert a line break every 76 characters, or  to not insert line breaks.</param>
    /// <returns>The string representation in base 64 of  elements of , starting at position .</returns>
    public static String ToBase64String(this Byte[] inArray, Int32 offset, Int32 length, Base64FormattingOptions options)
    {
        return Convert.ToBase64String(inArray, offset, length, options);
    }
    /// <summary>
    ///     Converts a subset of an 8-bit unsigned integer array to an equivalent subset of a Unicode character array
    ///     encoded with base-64 digits. Parameters specify the subsets as offsets in the input and output arrays, and
    ///     the number of elements in the input array to convert.
    /// </summary>
    /// <param name="inArray">An input array of 8-bit unsigned integers.</param>
    /// <param name="offsetIn">A position within .</param>
    /// <param name="length">The number of elements of  to convert.</param>
    /// <param name="outArray">An output array of Unicode characters.</param>
    /// <param name="offsetOut">A position within .</param>
    /// <returns>A 32-bit signed integer containing the number of bytes in .</returns>
    public static Int32 ToBase64CharArray(this Byte[] inArray, Int32 offsetIn, Int32 length, Char[] outArray, Int32 offsetOut)
    {
        return Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut);
    }

    /// <summary>
    ///     Converts a subset of an 8-bit unsigned integer array to an equivalent subset of a Unicode character array
    ///     encoded with base-64 digits. Parameters specify the subsets as offsets in the input and output arrays, the
    ///     number of elements in the input array to convert, and whether line breaks are inserted in the output array.
    /// </summary>
    /// <param name="inArray">An input array of 8-bit unsigned integers.</param>
    /// <param name="offsetIn">A position within .</param>
    /// <param name="length">The number of elements of  to convert.</param>
    /// <param name="outArray">An output array of Unicode characters.</param>
    /// <param name="offsetOut">A position within .</param>
    /// <param name="options">to insert a line break every 76 characters, or  to not insert line breaks.</param>
    /// <returns>A 32-bit signed integer containing the number of bytes in .</returns>
    public static Int32 ToBase64CharArray(this Byte[] inArray, Int32 offsetIn, Int32 length, Char[] outArray, Int32 offsetOut, Base64FormattingOptions options)
    {
        return Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut, options);
    }
    /// <summary>
    ///     A byte[] extension method that resizes the byte[].
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="newSize">Size of the new.</param>
    /// <returns>A byte[].</returns>
    public static byte[] Resize(this byte[] @this, int newSize)
    {
        Array.Resize(ref @this, newSize);
        return @this;
    }

    #endregion



    /// <summary>
    /// 转换为Base62编码字节数组
    /// </summary>
    /// <param name="base62"></param>
    /// <returns></returns>
    public static byte[] FromBase62(string base62)
    {
        if (base62 == null)
            return null;

        if (base62.Length == 0)
            return new byte[0];

        int len1 = base62.Length - 1;

        int count = 0;

        var stream = new BitStream(base62.Length * 6 / 8);

        foreach (char c in base62)
        {
            // Look up coding table
            if (c < '0' || c > 'z')
                return null;

            int index = Base62Index.IndexOf(c);

            // If end is reached
            if (count == len1)
            {
                // Check if the ending is good
                int mod = (int)(stream.Position % 8);
                if (mod == 0)
                    return null;

                if ((index >> (8 - mod)) > 0)
                    return null;

                stream.Write(new byte[] { (byte)(index << mod) }, 0, 8 - mod);
            }
            else
            {
                // If 60 or 61 then only write 5 bits to the stream, otherwise 6 bits.
                if (index == 60)
                {
                    stream.Write(new byte[] { 0xf0 }, 0, 5);
                }
                else if (index == 61)
                {
                    stream.Write(new byte[] { 0xf8 }, 0, 5);
                }
                else
                {
                    stream.Write(new byte[] { (byte)index }, 2, 6);
                }
            }
            count++;
        }

        // Dump out the bytes
        byte[] result = new byte[stream.Position / 8];
        stream.Seek(0, SeekOrigin.Begin);
        stream.Read(result, 0, result.Length * 8);

        return result;
    }
    /// <summary>
    /// 转换为Base62编码字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToBase62(this byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return "";

        // https://github.com/renmengye/base62-csharp/
        StringBuilder sb = new StringBuilder(bytes.Length * 3 / 2);

        var stream = new BitStream(bytes);         // Set up the BitStream
        var b = new byte[1];                          // Only read 6-bit at a time
        while (true)
        {
            b[0] = 0;
            int length = stream.Read(b, 0, 6);           // Try to read 6 bits
            if (length == 6)                                // Not reaching the end
            {
                if ((int)(b[0] >> 3) == 0x1f)            // First 5-bit is 11111
                {
                    sb.Append(Base62Index[61]);
                    stream.Seek(-1, SeekOrigin.Current);    // Leave the 6th bit to next group
                }
                else if ((int)(b[0] >> 3) == 0x1e)       // First 5-bit is 11110
                {
                    sb.Append(Base62Index[60]);
                    stream.Seek(-1, SeekOrigin.Current);
                }
                else                                        // Encode 6-bit
                {
                    sb.Append(Base62Index[(int)(b[0] >> 2)]);
                }
            }
            else if (length == 0)                           // Reached the end completely
            {
                break;
            }
            else                                            // Reached the end with some bits left
            {
                // Padding 0s to make the last bits to 6 bit
                sb.Append(Base62Index[(int)(b[0] >> (int)(8 - length))]);
                break;
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// 转换为Base64编码字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToBase64(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }
    /// <summary>
    /// 转换为Base64编码字节数组
    /// </summary>
    /// <param name="base64"></param>
    /// <returns></returns>
    public static byte[] FromBase64(string base64)
    {
        return Convert.FromBase64String(base64);
    }

    /// <summary>
    /// 转换为Hex编码字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToHexEncoding(this byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return "";

        var result = new char[bytes.Length * 2];
        int resultPos = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            var encodedByte = _byteMap[bytes[i]];
            result[resultPos] = encodedByte[0];
            result[resultPos + 1] = encodedByte[1];
            resultPos += 2;
        }
        return new string(result);
    }

    public static byte[] FromHexEncoding(this string hexEncoded)
    {
        if (string.IsNullOrWhiteSpace(hexEncoded))
            return null;

        hexEncoded = hexEncoded.Trim();

        byte[] result = new byte[hexEncoded.Length / 2];

        int bpos = 0;
        for (int i = 0; i < hexEncoded.Length; i += 2)
        {
            var hc = hexEncoded[i];
            if (hc >= _toByteMap.Length)
                return null;
            var hb = _toByteMap[(int)hc];
            if (hb == -1)
                return null;

            int lb = 0;
            if (i + 1 < hexEncoded.Length)
            {
                var lc = hexEncoded[i + 1];
                lb = _toByteMap[(int)lc];
            }

            result[bpos++] = (byte)((hb << 4) + lb);
        }

        return result;
    }

    /// <summary>
    /// 转换为UTF-8 编码字符串
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToStringFromUTF8(this byte[] bytes)
    {
        if (bytes == null)
            return null;
        if (bytes.Length == 0)
            return "";
        return UTF8Encoding.UTF8.GetString(bytes);
    }

    public static byte[] ToUTF8(this string str)
    {
        if (str == null)
            return null;
        if (str.Length == 0)
            return new byte[0];

        return UTF8Encoding.UTF8.GetBytes(str);
    }

    /// <summary>
    /// 比较是否相等
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="bytes2"></param>
    /// <returns></returns>
    public static bool IsEqual(this byte[] bytes, byte[] bytes2)
    {
        if (bytes == null || bytes.Length == 0)
        {
            if (bytes2 == null || bytes2.Length == 0)
                return true;
            return false;
        }
        else if (bytes2 == null || bytes2.Length == 0)
            return false;

        if (bytes.Length != bytes2.Length)
            return false;

        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] != bytes2[i])
                return false;
        }
        return true;
    }

}