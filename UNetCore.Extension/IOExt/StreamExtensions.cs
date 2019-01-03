using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

    /// <summary>
    /// 	Extension methods any kind of streams
    /// </summary>
    public static class StreamExtensions
    {
       
        public static void Read(this Stream stream, Action<byte[], int, int> readAction, int bufferSize = 0x5000)
        {
            if (!stream.CanRead)
            {
                throw new InvalidOperationException(Guard.GetString("StreamNotSupportRead", new object[0]));
            }
            int num = 0;
            int num2 = 0;
            byte[] buffer = new byte[bufferSize];
            while ((num2 = stream.Read(buffer, 0, bufferSize)) > 0)
            {
                readAction(buffer, num, num2);
                num += num2;
            }
        }

        public static Stream WriteTo(this Stream stream, Stream targetStream, int bufferSize = 0x5000)
        {
            if (!stream.CanRead)
            {
                throw new InvalidOperationException(Guard.GetString("StreamNotSupportRead", new object[0]));
            }
            if (!targetStream.CanWrite)
            {
                throw new InvalidOperationException(Guard.GetString("StreamNotSupportWrite", new object[0]));
            }
            byte[] buffer = new byte[bufferSize];
            int count = 0;
            while ((count = stream.Read(buffer, 0, bufferSize)) > 0)
            {
                targetStream.Write(buffer, 0, count);
            }
            return stream;
        }

        /// <summary>
        /// 	Opens a StreamReader using the default encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The stream reader</returns>
        public static StreamReader GetReader(this Stream stream)
        {
            return stream.GetReader(null);
        }

        /// <summary>
        /// 	Opens a StreamReader using the specified encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <param name = "encoding">The encoding.</param>
        /// <returns>The stream reader</returns>
        public static StreamReader GetReader(this Stream stream, Encoding encoding)
        {
            if (stream.CanRead == false)
                throw new InvalidOperationException("Stream does not support reading.");

            encoding = (encoding ?? Encoding.Default);
            return new StreamReader(stream, encoding);
        }

        /// <summary>
        /// 	Opens a StreamWriter using the default encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The stream writer</returns>
        public static StreamWriter GetWriter(this Stream stream)
        {
            return stream.GetWriter(null);
        }

        /// <summary>
        /// 	Opens a StreamWriter using the specified encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <param name = "encoding">The encoding.</param>
        /// <returns>The stream writer</returns>
        public static StreamWriter GetWriter(this Stream stream, Encoding encoding)
        {
            if (stream.CanWrite == false)
                throw new InvalidOperationException("Stream does not support writing.");

            encoding = (encoding ?? Encoding.Default);
            return new StreamWriter(stream, encoding);
        }

        /// <summary>
        /// 	Reads all text from the stream using the default encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The result string.</returns>
        public static string ReadToEnd(this Stream stream)
        {
            return stream.ReadToEnd(null);
        }

        /// <summary>
        /// 	Reads all text from the stream using a specified encoding.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <param name = "encoding">The encoding.</param>
        /// <returns>The result string.</returns>
        public static string ReadToEnd(this Stream stream, Encoding encoding)
        {
            using (var reader = stream.GetReader(encoding))
                return reader.ReadToEnd();
        }

        /// <summary>
        /// 	Sets the stream cursor to the beginning of the stream.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The stream</returns>
        public static Stream SeekToBegin(this Stream stream)
        {
            if (stream.CanSeek == false)
                throw new InvalidOperationException("Stream does not support seeking.");

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// 	Sets the stream cursor to the end of the stream.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The stream</returns>
        public static Stream SeekToEnd(this Stream stream)
        {
            if (stream.CanSeek == false)
                throw new InvalidOperationException("Stream does not support seeking.");

            stream.Seek(0, SeekOrigin.End);
            return stream;
        }

        /// <summary>
        /// 	Copies one stream into a another one.
        /// </summary>
        /// <param name = "stream">The source stream.</param>
        /// <param name = "targetStream">The target stream.</param>
        /// <returns>The source stream.</returns>
        public static Stream CopyTo(this Stream stream, Stream targetStream)
        {
              stream.CopyTo(targetStream, 4096);
              return targetStream;
        }
        /// <summary>
        ///     A byte[] extension method that converts the @this to a memory stream.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a MemoryStream.</returns>
        public static MemoryStream ToMemoryStream(this byte[] @this)
        {
            return new MemoryStream(@this);
        }
        /// <summary>
        /// 	Copies one stream into a another one.
        /// </summary>
        /// <param name = "stream">The source stream.</param>
        /// <param name = "targetStream">The target stream.</param>
        /// <param name = "bufferSize">The buffer size used to read / write.</param>
        /// <returns>The source stream.</returns>
        public static Stream CopyTo(this Stream stream, Stream targetStream, int bufferSize)
        {
            if (stream.CanRead == false)
                throw new InvalidOperationException("Source stream does not support reading.");
            if (targetStream.CanWrite == false)
                throw new InvalidOperationException("Target stream does not support writing.");

            var buffer = new byte[bufferSize];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, bufferSize)) > 0)
                targetStream.Write(buffer, 0, bytesRead);
            return stream;
        }

        /// <summary>
        /// 	Copies any stream into a local MemoryStream
        /// </summary>
        /// <param name = "stream">The source stream.</param>
        /// <returns>The copied memory stream.</returns>
        public static MemoryStream CopyToMemory(this Stream stream)
        {
            var memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);
            return memoryStream;
        }

        /// <summary>
        /// 	Reads the entire stream and returns a byte array.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <returns>The byte array</returns>
        /// <remarks>
        /// 	Thanks to EsbenCarlsen  for providing an update to this method.
        /// </remarks>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            using (var memoryStream = CopyToMemory(stream))
                return memoryStream.ToArray();
        }

        /// <summary>
        /// 	Reads a fixed number of bytes.
        /// </summary>
        /// <param name = "stream">The stream to read from</param>
        /// <param name = "bufsize">The number of bytes to read.</param>
        /// <returns>the read byte[]</returns>
        public static byte[] ReadFixedBuffersize(this Stream stream, int bufsize)
        {
            var buf = new byte[bufsize];
            int offset = 0, cnt;
            do
            {
                cnt = stream.Read(buf, offset, bufsize - offset);
                if (cnt == 0)
                    return null;
                offset += cnt;
            } while (offset < bufsize);

            return buf;
        }

        /// <summary>
        /// 	Writes all passed bytes to the specified stream.
        /// </summary>
        /// <param name = "stream">The stream.</param>
        /// <param name = "bytes">The byte array / buffer.</param>
        public static void Write(this Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
        /// <summary>
        ///     A Stream extension method that converts the @this to a md 5 hash.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a string.</returns>
        public static string ToMD5Hash(this Stream @this)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(@this);
                var sb = new StringBuilder();
                foreach (byte bytes in hashBytes)
                {
                    sb.Append(bytes.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        ///     A Stream extension method that converts the Stream to a byte array.
        /// </summary>
        /// <param name="this">The Stream to act on.</param>
        /// <returns>The Stream as a byte[].</returns>
        public static byte[] ToByteArray(this Stream @this)
        {
            using (var ms = new MemoryStream())
            {
                @this.CopyTo(ms);
                return ms.ToArray();
            }
        }
  
 
        /// <summary>
        ///     A Stream extension method that reads a stream to the end.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        ///     The rest of the stream as a string, from the current position to the end. If the current position is at the
        ///     end of the stream, returns an empty string ("").
        /// </returns>
        public static string ReadToEnd(this Stream @this, long position)
        {
            @this.Position = position;

            using (var sr = new StreamReader(@this, Encoding.Default))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        ///     A Stream extension method that reads a stream to the end.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        ///     The rest of the stream as a string, from the current position to the end. If the current position is at the
        ///     end of the stream, returns an empty string ("").
        /// </returns>
        public static string ReadToEnd(this Stream @this, Encoding encoding, long position)
        {
            @this.Position = position;

            using (var sr = new StreamReader(@this, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        ///     A string extension method that converts the @this to a byte array.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a byte[].</returns>
        public static byte[] ToByteArray(this string @this)
        {
            Encoding encoding = Activator.CreateInstance<ASCIIEncoding>();
            return encoding.GetBytes(@this);
        }
        /// <summary>
        ///     A string extension method that converts the @this to a byte array.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a byte[].</returns>
        public static byte[] ToByteArray(this string @this, Encoding encoding)
        {
            return encoding.GetBytes(@this);
        }
        /// <summary>
        ///     A string extension method that save the string into a file.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="fileName">Filename of the file.</param>
        /// <param name="append">(Optional) if the text should be appended to file file if it's exists.</param>
        public static void SaveAs(this string @this, string fileName, bool append = false)
        {
            using (TextWriter tw = new StreamWriter(fileName, append))
            {
                tw.Write(@this);
            }
        }

        /// <summary>
        ///     A string extension method that save the string into a file.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="file">The FileInfo.</param>
        /// <param name="append">(Optional) if the text should be appended to file file if it's exists.</param>
        public static void SaveAs(this string @this, FileInfo file, bool append = false)
        {
            using (TextWriter tw = new StreamWriter(file.FullName, append))
            {
                tw.Write(@this);
            }
        }


        /// <summary>
        /// 判断文件流是否为UTF8字符集
        /// </summary>
        /// <param name="sbInputStream">文件流</param>
        /// <returns>判断结果</returns>
        public static bool IsUTF8(this System.IO.FileStream sbInputStream)
        {
            int i;
            byte cOctets;  // octets to go in this UTF-8 encoded character 
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;

            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                chr = (byte)sbInputStream.ReadByte();

                if ((chr & 0x80) != 0) bAllAscii = false;

                if (cOctets == 0)
                {
                    if (chr >= 0x80)
                    {
                        do
                        {
                            chr <<= 1;
                            cOctets++;
                        }
                        while ((chr & 0x80) != 0);

                        cOctets--;
                        if (cOctets == 0) return false;
                    }
                }
                else
                {
                    if ((chr & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    cOctets--;
                }
            }

            if (cOctets > 0)
            {
                return false;
            }

            if (bAllAscii)
            {
                return false;
            }

            return true;

        }

    }