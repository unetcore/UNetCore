using System;
using System.Text;

using System.IO;
using System.Security.Cryptography;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;

public static class EncryptExtensions
{
     /// <summary>
    /// MD5 加密静态方法
     /// </summary>
     /// <param name="value"></param>
     /// <returns></returns>
    public static string ToEncryptMd5String(this string value)
    {
        return PasswordHelper.MD5Encrypt(value);
    }

    /// <summary>
    ///  DES 加密(数据加密标准，速度较快，适用于加密大量数据的场合)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ToEncryptDesString(this string value, string key)
    {
        return PasswordHelper.DESEncrypt(value, key);
    }
    /// <summary>
    /// DES 解密(数据加密标准，速度较快，适用于加密大量数据的场合)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ToDecryptDesString(this string value, string key)
    {
        return PasswordHelper.DESDecrypt(value, key);
    }
    /// <summary>
    /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ToEncryptAesString(this string value, string key)
    {
        return PasswordHelper.AESEncrypt(value, key);
    }
    /// <summary>
    /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ToDecryptAesString(this string value, string key)
    {
        return PasswordHelper.AESDecrypt(value, key);
    }

    /// <summary>
    /// RC2 加密(用变长密钥对大量数据进行加密)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ToEncryptRc2String(this string value, string key)
    {
        return PasswordHelper.RC2Encrypt(value, key);
    }
    /// <summary>
    /// RC2 解密(用变长密钥对大量数据进行加密)
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ToDecryptRc2String(this string value, string key)
    {
        return PasswordHelper.RC2Decrypt(value, key);
    }


    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ToEncryptBase64String(this string value)
    {
        return PasswordHelper.Base64_Encode(value);
    }
    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string ToDecryptBase64String(this string value)
    {
        return PasswordHelper.Base64_Decode(value);
    }


    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name = "value">The input value.</param>
    /// <returns>The Base 64 encoded string</returns>
    public static string ToEncodeBase64(this string value)
    {
        return value.ToEncodeBase64(null);
    }

    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name = "value">The input value.</param>
    /// <param name = "encoding">The encoding.</param>
    /// <returns>The Base 64 encoded string</returns>
    public static string ToEncodeBase64(this string value, Encoding encoding)
    {
        encoding = (encoding ?? Encoding.UTF8);
        var bytes = encoding.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name = "encodedValue">The Base 64 encoded value.</param>
    /// <returns>The decoded string</returns>
    public static string ToDecodeBase64(this string encodedValue)
    {
        return encodedValue.ToDecodeBase64(null);
    }

    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name = "encodedValue">The Base 64 encoded value.</param>
    /// <param name = "encoding">The encoding.</param>
    /// <returns>The decoded string</returns>
    public static string ToDecodeBase64(this string encodedValue, Encoding encoding)
    {
        encoding = (encoding ?? Encoding.UTF8);
        var bytes = Convert.FromBase64String(encodedValue);
        return encoding.GetString(bytes);
    }

    /// <summary>
    ///  转换为RSA加密字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="key">The key.</param>
    /// <returns>The encrypted string.</returns>
    public static string ToEncryptRSA(this string @this, string key)
    {
        var cspp = new CspParameters { KeyContainerName = key };
        var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
        byte[] bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(@this), true);

        return BitConverter.ToString(bytes);
    }
    /// <summary>
    /// 解密RSA字符串
    /// </summary>
    /// <param name="this">The @this to act on.</param>
    /// <param name="key">The key.</param>
    /// <returns>The decrypted string.</returns>
    public static string DecryptRSA(this string @this, string key)
    {
        var cspp = new CspParameters { KeyContainerName = key };
        var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
        string[] decryptArray = @this.Split(new[] { "-" }, StringSplitOptions.None);
        byte[] decryptByteArray = Array.ConvertAll(decryptArray, (s => Convert.ToByte(byte.Parse(s, NumberStyles.HexNumber))));
        byte[] bytes = rsa.Decrypt(decryptByteArray, true);

        return Encoding.UTF8.GetString(bytes);
    }


    /// <summary>
    /// MD5字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string MD5(this string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
        string str2 = "";
        for (int i = 0; i < bytes.Length; i++)
        {
            str2 = str2 + bytes[i].ToString("x").PadLeft(2, '0');
        }
        return str2;
    }
    /// <summary>
    /// 转换为16位MD5字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string MD5_16(this string str)
    {
        return MD5(str).Substring(8, 0x10);
    }
    /// <summary>
    /// 转换为16位MD5字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToEncryptMd5_16(this string str)
    {
        return MD5(str).Substring(8, 0x10);
    }
}


public class PasswordHelper
{
    public static string EncryptPassword(string value, string EecryptKey, string type)
    {
        try
        {
            switch (type.ToUpper())
            {
                case "MD5":
                    return MD5Encrypt(value);
                case "RC2":
                    return RC2Encrypt(value, EecryptKey);
                case "DES":
                    return DESEncrypt(value, EecryptKey);
                case "AES":
                    return AESEncrypt(value, EecryptKey);

                default:
                    return MD5Encrypt(value);
            }
        }
        catch (Exception ex)
        {

            throw new Exception("PasswordHelper.EncryptPassword加密出错：" + ex.Message);
        }
    }


    // 创建Key
    public static string GenerateKey()
    {
        DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
        return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
    }

    /// <summary>
    /// MD5 加密静态方法
    /// </summary>
    /// <param name="EncryptString">待加密的密文</param>
    /// <returns>returns</returns>
    public static string MD5Encrypt(string EncryptString)
    {
        if (string.IsNullOrEmpty(EncryptString)) { throw (new Exception("密文不得为空")); }

        MD5 m_ClassMD5 = new MD5CryptoServiceProvider();

        string m_strEncrypt = "";

        try
        {
            m_strEncrypt = BitConverter.ToString(m_ClassMD5.ComputeHash(Encoding.Default.GetBytes(EncryptString))).Replace("-", "");
        }
        catch (ArgumentException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_ClassMD5.Clear(); }

        return m_strEncrypt;
    }


    /// <summary>
    /// DES 加密(数据加密标准，速度较快，适用于加密大量数据的场合)
    /// </summary>
    /// <param name="EncryptString">待加密的密文</param>
    /// <param name="EncryptKey">加密的密钥</param>
    /// <returns>returns</returns>
    public static string DESEncrypt(string EncryptString, string EncryptKey)
    {
        if (string.IsNullOrEmpty(EncryptString)) { throw (new Exception("密文不得为空")); }

        if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

        if (EncryptKey.Length != 8) { throw (new Exception("密钥必须为8位")); }

        byte[] m_btIV = { 0x12, 0x55, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0x41, 0xAB };//0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF

        string m_strEncrypt = "";

        DESCryptoServiceProvider m_DESProvider = new DESCryptoServiceProvider();

        try
        {
            byte[] m_btEncryptString = Encoding.Default.GetBytes(EncryptString);

            MemoryStream m_stream = new MemoryStream();

            CryptoStream m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateEncryptor(Encoding.Default.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);

            m_cstream.Write(m_btEncryptString, 0, m_btEncryptString.Length);

            m_cstream.FlushFinalBlock();

            m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());

            m_stream.Close(); m_stream.Dispose();

            m_cstream.Close(); m_cstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_DESProvider.Clear(); }

        return m_strEncrypt;
    }
    /// <summary>
    /// DES 解密(数据加密标准，速度较快，适用于加密大量数据的场合)
    /// </summary>
    /// <param name="DecryptString">待解密的密文</param>
    /// <param name="DecryptKey">解密的密钥</param>
    /// <returns>returns</returns>
    public static string DESDecrypt(string DecryptString, string DecryptKey)
    {
        if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }

        if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

        if (DecryptKey.Length != 8) { throw (new Exception("密钥必须为8位")); }

        byte[] m_btIV = { 0x12, 0x55, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0x41, 0xAB };// 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF

        string m_strDecrypt = "";

        DESCryptoServiceProvider m_DESProvider = new DESCryptoServiceProvider();

        try
        {
            byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);

            MemoryStream m_stream = new MemoryStream();

            CryptoStream m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);

            m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);

            m_cstream.FlushFinalBlock();

            m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());

            m_stream.Close(); m_stream.Dispose();

            m_cstream.Close(); m_cstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_DESProvider.Clear(); }

        return m_strDecrypt;
    }
    /// <summary>
    /// RC2 加密(用变长密钥对大量数据进行加密)
    /// </summary>
    /// <param name="EncryptString">待加密密文</param>
    /// <param name="EncryptKey">加密密钥</param>
    /// <returns>returns</returns>
    public static string RC2Encrypt(string EncryptString, string EncryptKey)
    {
        if (string.IsNullOrEmpty(EncryptString)) { throw (new Exception("密文不得为空")); }

        if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

        if (EncryptKey.Length < 5 || EncryptKey.Length > 16) { throw (new Exception("密钥必须为5-16位")); }

        string m_strEncrypt = "";

        byte[] m_btIV = { 0x12, 0x55, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0x41, 0xAB };//0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF

        RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();

        try
        {
            byte[] m_btEncryptString = Encoding.Default.GetBytes(EncryptString);

            MemoryStream m_stream = new MemoryStream();

            CryptoStream m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateEncryptor(Encoding.Default.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);

            m_cstream.Write(m_btEncryptString, 0, m_btEncryptString.Length);

            m_cstream.FlushFinalBlock();

            m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());

            m_stream.Close(); m_stream.Dispose();

            m_cstream.Close(); m_cstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_RC2Provider.Clear(); }

        return m_strEncrypt;
    }
    /// <summary>
    /// RC2 解密(用变长密钥对大量数据进行加密)
    /// </summary>
    /// <param name="DecryptString">待解密密文</param>
    /// <param name="DecryptKey">解密密钥</param>
    /// <returns>returns</returns>
    public static string RC2Decrypt(string DecryptString, string DecryptKey)
    {
        if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }

        if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

        if (DecryptKey.Length < 5 || DecryptKey.Length > 16) { throw (new Exception("密钥必须为5-16位")); }

        byte[] m_btIV = { 0x12, 0x55, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0x41, 0xAB };//0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF

        string m_strDecrypt = "";

        RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();

        try
        {
            byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);

            MemoryStream m_stream = new MemoryStream();

            CryptoStream m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);

            m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);

            m_cstream.FlushFinalBlock();

            m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());

            m_stream.Close(); m_stream.Dispose();

            m_cstream.Close(); m_cstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_RC2Provider.Clear(); }
        return m_strDecrypt;
    }
    /// <summary>
    /// 3DES 加密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
    /// </summary>
    /// <param name="EncryptString">待加密密文</param>
    /// <param name="EncryptKey1">密钥一</param>
    /// <param name="EncryptKey2">密钥二</param>
    /// <param name="EncryptKey3">密钥三</param>
    /// <returns>returns</returns>
    public static string DES3Encrypt(string EncryptString, string EncryptKey1, string EncryptKey2, string EncryptKey3)
    {
        string m_strEncrypt = "";

        try
        {
            m_strEncrypt = DESEncrypt(EncryptString, EncryptKey3);

            m_strEncrypt = DESEncrypt(m_strEncrypt, EncryptKey2);

            m_strEncrypt = DESEncrypt(m_strEncrypt, EncryptKey1);
        }
        catch (Exception ex) { throw ex; }

        return m_strEncrypt;
    }
    /// <summary>
    /// 3DES 解密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
    /// </summary>
    /// <param name="DecryptString">待解密密文</param>
    /// <param name="DecryptKey1">密钥一</param>
    /// <param name="DecryptKey2">密钥二</param>
    /// <param name="DecryptKey3">密钥三</param>
    /// <returns>returns</returns>
    public static string DES3Decrypt(string DecryptString, string DecryptKey1, string DecryptKey2, string DecryptKey3)
    {
        string m_strDecrypt = "";

        try
        {
            m_strDecrypt = DESDecrypt(DecryptString, DecryptKey1);

            m_strDecrypt = DESDecrypt(m_strDecrypt, DecryptKey2);

            m_strDecrypt = DESDecrypt(m_strDecrypt, DecryptKey3);
        }
        catch (Exception ex) { throw ex; }

        return m_strDecrypt;
    }
    /// <summary>
    /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="EncryptString">待加密密文</param>
    /// <param name="EncryptKey">加密密钥</param>
    /// <returns></returns>
    public static string AESEncrypt(string EncryptString, string EncryptKey)
    {
        if (string.IsNullOrEmpty(EncryptString)) { throw (new Exception("密文不得为空")); }

        if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }

        string m_strEncrypt = "";

        byte[] m_btIV = Convert.FromBase64String("Uox4jvUy/ye7Cd7k89QQgQas");//

        Rijndael m_AESProvider = Rijndael.Create();

        try
        {
            byte[] m_btEncryptString = Encoding.Default.GetBytes(EncryptString);

            MemoryStream m_stream = new MemoryStream();

            CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateEncryptor(Encoding.Default.GetBytes(EncryptKey), m_btIV), CryptoStreamMode.Write);

            m_csstream.Write(m_btEncryptString, 0, m_btEncryptString.Length); m_csstream.FlushFinalBlock();

            m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());

            m_stream.Close(); m_stream.Dispose();

            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }

        return m_strEncrypt;
    }
    /// <summary>
    /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
    /// </summary>
    /// <param name="DecryptString">待解密密文</param>
    /// <param name="DecryptKey">解密密钥</param>
    /// <returns></returns>
    public static string AESDecrypt(string DecryptString, string DecryptKey)
    {
        if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }

        if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }

        string m_strDecrypt = "";

        byte[] m_btIV = Convert.FromBase64String("Uox4jvUy/ye7Cd7k89QQgQas");//Rkb4jvUy/ye7Cd7k89QQgQ==

        Rijndael m_AESProvider = Rijndael.Create();

        try
        {
            byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);

            MemoryStream m_stream = new MemoryStream();

            CryptoStream m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);

            m_csstream.Write(m_btDecryptString, 0, m_btDecryptString.Length); m_csstream.FlushFinalBlock();

            m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());

            m_stream.Close(); m_stream.Dispose();

            m_csstream.Close(); m_csstream.Dispose();
        }
        catch (IOException ex) { throw ex; }
        catch (CryptographicException ex) { throw ex; }
        catch (ArgumentException ex) { throw ex; }
        catch (Exception ex) { throw ex; }
        finally { m_AESProvider.Clear(); }

        return m_strDecrypt;
    }


    #region Base64加密
    /// <summary>
    /// Base64加密
    /// </summary>
    /// <param name="text">要加密的字符串</param>
    /// <returns></returns>
    public static string EncodeBase64(string text)
    {
        //如果字符串为空，则返回
        if (string.IsNullOrEmpty(text))
        {
            return "";
        }

        try
        {
            char[] Base64Code = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
											'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
											'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
											'8','9','+','/','='};
            byte empty = (byte)0;
            ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
            StringBuilder outmessage;
            int messageLen = byteMessage.Count;
            int page = messageLen / 3;
            int use = 0;
            if ((use = messageLen % 3) > 0)
            {
                for (int i = 0; i < 3 - use; i++)
                    byteMessage.Add(empty);
                page++;
            }
            outmessage = new System.Text.StringBuilder(page * 4);
            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[3];
                instr[0] = (byte)byteMessage[i * 3];
                instr[1] = (byte)byteMessage[i * 3 + 1];
                instr[2] = (byte)byteMessage[i * 3 + 2];
                int[] outstr = new int[4];
                outstr[0] = instr[0] >> 2;
                outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                if (!instr[1].Equals(empty))
                    outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                else
                    outstr[2] = 64;
                if (!instr[2].Equals(empty))
                    outstr[3] = (instr[2] & 0x3f);
                else
                    outstr[3] = 64;
                outmessage.Append(Base64Code[outstr[0]]);
                outmessage.Append(Base64Code[outstr[1]]);
                outmessage.Append(Base64Code[outstr[2]]);
                outmessage.Append(Base64Code[outstr[3]]);
            }
            return outmessage.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Base64解密
    /// <summary>
    /// Base64解密
    /// </summary>
    /// <param name="text">要解密的字符串</param>
    public static string DecodeBase64(string text)
    {
        //如果字符串为空，则返回
        if (string.IsNullOrEmpty(text))
        {
            return "";
        }

        //将空格替换为加号
        text = text.Replace(" ", "+");

        try
        {
            if ((text.Length % 4) != 0)
            {
                return "包含不正确的BASE64编码";
            }
            if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
            {
                return "包含不正确的BASE64编码";
            }
            string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            int page = text.Length / 4;
            ArrayList outMessage = new ArrayList(page * 3);
            char[] message = text.ToCharArray();
            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[4];
                instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                byte[] outstr = new byte[3];
                outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                if (instr[2] != 64)
                {
                    outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                }
                else
                {
                    outstr[2] = 0;
                }
                if (instr[3] != 64)
                {
                    outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                }
                else
                {
                    outstr[2] = 0;
                }
                outMessage.Add(outstr[0]);
                if (outstr[1] != 0)
                    outMessage.Add(outstr[1]);
                if (outstr[2] != 0)
                    outMessage.Add(outstr[2]);
            }
            byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
            return Encoding.Default.GetString(outbyte);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region TripleDES加密
    /// <summary>
    /// TripleDES加密
    /// </summary>
    public static string TripleDESEncrypting(string strSource)
    {
        try
        {
            byte[] bytIn = Encoding.Default.GetBytes(strSource);
            byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
            byte[] IV = { 55, 103, 246, 79, 36, 99, 167, 3 };  //定义偏移量
            TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
            TripleDES.IV = IV;
            TripleDES.Key = key;
            ICryptoTransform encrypto = TripleDES.CreateEncryptor();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            byte[] bytOut = ms.ToArray();
            return System.Convert.ToBase64String(bytOut);
        }
        catch (Exception ex)
        {
            throw new Exception("加密时候出现错误!错误提示:\n" + ex.Message);
        }
    }
    #endregion

    #region TripleDES解密
    /// <summary>
    /// TripleDES解密
    /// </summary>
    public static string TripleDESDecrypting(string Source)
    {
        try
        {
            byte[] bytIn = System.Convert.FromBase64String(Source);
            byte[] key = { 42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119, 35, 184, 197 }; //定义密钥
            byte[] IV = { 55, 103, 246, 79, 36, 99, 167, 3 };   //定义偏移量
            TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
            TripleDES.IV = IV;
            TripleDES.Key = key;
            ICryptoTransform encrypto = TripleDES.CreateDecryptor();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader strd = new StreamReader(cs, Encoding.Default);
            return strd.ReadToEnd();
        }
        catch (Exception ex)
        {
            throw new Exception("解密时候出现错误!错误提示:\n" + ex.Message);
        }
    }
    #endregion

    #region RSA 加密解密

    #region RSA 的密钥产生

    /// <summary>
    /// RSA 的密钥产生 产生私钥 和公钥 
    /// </summary>
    /// <param name="xmlKeys"></param>
    /// <param name="xmlPublicKey"></param>
    public void RSAKey(out string xmlKeys, out string xmlPublicKey)
    {
        System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        xmlKeys = rsa.ToXmlString(true);
        xmlPublicKey = rsa.ToXmlString(false);
    }
    #endregion

    #region RSA的加密函数
    //############################################################################## 
    //RSA 方式加密 
    //说明KEY必须是XML的行式,返回的是字符串 
    //在有一点需要说明！！该加密方式有 长度 限制的！！ 
    //############################################################################## 

    //RSA的加密函数  string
    public string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
    {

        byte[] PlainTextBArray;
        byte[] CypherTextBArray;
        string Result;
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(xmlPublicKey);
        PlainTextBArray = (new UnicodeEncoding()).GetBytes(m_strEncryptString);
        CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
        Result = Convert.ToBase64String(CypherTextBArray);
        return Result;

    }
    //RSA的加密函数 byte[]
    public string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
    {

        byte[] CypherTextBArray;
        string Result;
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(xmlPublicKey);
        CypherTextBArray = rsa.Encrypt(EncryptString, false);
        Result = Convert.ToBase64String(CypherTextBArray);
        return Result;

    }
    #endregion

    #region RSA的解密函数
    //RSA的解密函数  string
    public string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
    {
        byte[] PlainTextBArray;
        byte[] DypherTextBArray;
        string Result;
        System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(xmlPrivateKey);
        PlainTextBArray = Convert.FromBase64String(m_strDecryptString);
        DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
        Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
        return Result;

    }

    //RSA的解密函数  byte
    public string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
    {
        byte[] DypherTextBArray;
        string Result;
        System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(xmlPrivateKey);
        DypherTextBArray = rsa.Decrypt(DecryptString, false);
        Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
        return Result;

    }
    #endregion

    #endregion

    #region RSA数字签名

    #region 获取Hash描述表
    //获取Hash描述表 
    public bool GetHash(string m_strSource, ref byte[] HashData)
    {
        //从字符串中取得Hash描述 
        byte[] Buffer;
        System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
        Buffer = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
        HashData = MD5.ComputeHash(Buffer);

        return true;
    }

    //获取Hash描述表 
    public bool GetHash(string m_strSource, ref string strHashData)
    {

        //从字符串中取得Hash描述 
        byte[] Buffer;
        byte[] HashData;
        System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
        Buffer = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
        HashData = MD5.ComputeHash(Buffer);

        strHashData = Convert.ToBase64String(HashData);
        return true;

    }

    //获取Hash描述表 
    public bool GetHash(System.IO.FileStream objFile, ref byte[] HashData)
    {

        //从文件中取得Hash描述 
        System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
        HashData = MD5.ComputeHash(objFile);
        objFile.Close();

        return true;

    }

    //获取Hash描述表 
    public bool GetHash(System.IO.FileStream objFile, ref string strHashData)
    {

        //从文件中取得Hash描述 
        byte[] HashData;
        System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
        HashData = MD5.ComputeHash(objFile);
        objFile.Close();

        strHashData = Convert.ToBase64String(HashData);

        return true;

    }
    #endregion

    #region RSA签名
    //RSA签名 
    public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
    {

        System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

        RSA.FromXmlString(p_strKeyPrivate);
        System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
        //设置签名的算法为MD5 
        RSAFormatter.SetHashAlgorithm("MD5");
        //执行签名 
        EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

        return true;

    }

    //RSA签名 
    public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref string m_strEncryptedSignatureData)
    {

        byte[] EncryptedSignatureData;

        System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

        RSA.FromXmlString(p_strKeyPrivate);
        System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
        //设置签名的算法为MD5 
        RSAFormatter.SetHashAlgorithm("MD5");
        //执行签名 
        EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

        m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

        return true;

    }

    //RSA签名 
    public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref byte[] EncryptedSignatureData)
    {

        byte[] HashbyteSignature;

        HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
        System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

        RSA.FromXmlString(p_strKeyPrivate);
        System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
        //设置签名的算法为MD5 
        RSAFormatter.SetHashAlgorithm("MD5");
        //执行签名 
        EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

        return true;

    }

    //RSA签名 
    public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref string m_strEncryptedSignatureData)
    {

        byte[] HashbyteSignature;
        byte[] EncryptedSignatureData;

        HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
        System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

        RSA.FromXmlString(p_strKeyPrivate);
        System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
        //设置签名的算法为MD5 
        RSAFormatter.SetHashAlgorithm("MD5");
        //执行签名 
        EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);

        m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);

        return true;

    }
    #endregion

    #region RSA 签名验证

    public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
    {

        System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

        RSA.FromXmlString(p_strKeyPublic);
        System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
        //指定解密的时候HASH算法为MD5 
        RSADeformatter.SetHashAlgorithm("MD5");

        if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
    {

        byte[] HashbyteDeformatter;

        HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);

        System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

        RSA.FromXmlString(p_strKeyPublic);
        System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
        //指定解密的时候HASH算法为MD5 
        RSADeformatter.SetHashAlgorithm("MD5");

        if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
    {

        byte[] DeformatterData;

        System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

        RSA.FromXmlString(p_strKeyPublic);
        System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
        //指定解密的时候HASH算法为MD5 
        RSADeformatter.SetHashAlgorithm("MD5");

        DeformatterData = Convert.FromBase64String(p_strDeformatterData);

        if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
    {

        byte[] DeformatterData;
        byte[] HashbyteDeformatter;

        HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);
        System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();

        RSA.FromXmlString(p_strKeyPublic);
        System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
        //指定解密的时候HASH算法为MD5 
        RSADeformatter.SetHashAlgorithm("MD5");

        DeformatterData = Convert.FromBase64String(p_strDeformatterData);

        if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    #endregion

    public static string Base64_Decode(  string str)
    {
        byte[] bytes = Convert.FromBase64String(str);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string Base64_Encode(  string str)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
    }

    #endregion


}
