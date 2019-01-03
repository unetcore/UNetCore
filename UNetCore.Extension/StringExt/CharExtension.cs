
using System.Text;
    using System.Text.RegularExpressions;

    public static class CharExtension
    {
        /// <summary>
        /// 获取Ascii
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static int GetAsciiCode(this char character)
        {
            byte[] bytes = Encoding.GetEncoding(0).GetBytes(character.ToString());
            if (bytes.Length == 1)
            {
                return bytes[0];
            }
            return (((bytes[0] * 0x100) + bytes[1]) - 0x10000);
        }
        /// <summary>
        /// 是否中文字符串
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static bool IsChinese(this char character)
        {
            return Regex.IsMatch(character.ToString(), "^[一-龥]$");
        }
        /// <summary>
        /// 是否是行标识
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public static bool IsLine(this char character)
        {
            if (character != '\r')
            {
                return (character == '\n');
            }
            return true;
        }
    }


