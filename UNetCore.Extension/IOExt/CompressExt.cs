using System.IO;
using System.IO.Compression;
using System.Text;
/// <summary>
/// Compress Extensions
/// </summary>
    public static class CompressExt
    {
        /// <summary>
        ///     A byte[] extension method that decompress the byte array gzip to string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The byte array gzip to string.</returns>
        public static string DecompressGZip(this byte[] @this)
        {
            const int bufferSize = 1024;
            using (var memoryStream = new MemoryStream(@this))
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    // Memory stream for storing the decompressed bytes
                    using (var outStream = new MemoryStream())
                    {
                        var buffer = new byte[bufferSize];
                        int totalBytes = 0;
                        int readBytes;
                        while ((readBytes = zipStream.Read(buffer, 0, bufferSize)) > 0)
                        {
                            outStream.Write(buffer, 0, readBytes);
                            totalBytes += readBytes;
                        }
                        return Encoding.Default.GetString(outStream.GetBuffer(), 0, totalBytes);
                    }
                }
            }
        }

        /// <summary>
        ///     A byte[] extension method that decompress the byte array gzip to string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The byte array gzip to string.</returns>
        public static string DecompressGZip(this byte[] @this, Encoding encoding)
        {
            const int bufferSize = 1024;
            using (var memoryStream = new MemoryStream(@this))
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    // Memory stream for storing the decompressed bytes
                    using (var outStream = new MemoryStream())
                    {
                        var buffer = new byte[bufferSize];
                        int totalBytes = 0;
                        int readBytes;
                        while ((readBytes = zipStream.Read(buffer, 0, bufferSize)) > 0)
                        {
                            outStream.Write(buffer, 0, readBytes);
                            totalBytes += readBytes;
                        }
                        return encoding.GetString(outStream.GetBuffer(), 0, totalBytes);
                    }
                }
            }
        }

        /// <summary>
        ///     A FileInfo extension method that creates a zip file.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void CreateGZip(this FileInfo @this)
        {
            using (FileStream originalFileStream = @this.OpenRead())
            {
                using (FileStream compressedFileStream = File.Create(@this.FullName + ".gz"))
                {
                    using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        /// <summary>
        ///     A FileInfo extension method that creates a zip file.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destination">Destination for the zip.</param>
        public static void CreateGZip(this FileInfo @this, string destination)
        {
            using (FileStream originalFileStream = @this.OpenRead())
            {
                using (FileStream compressedFileStream = File.Create(destination))
                {
                    using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        /// <summary>
        ///     A FileInfo extension method that creates a zip file.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destination">Destination for the zip.</param>
        public static void CreateGZip(this FileInfo @this, FileInfo destination)
        {
            using (FileStream originalFileStream = @this.OpenRead())
            {
                using (FileStream compressedFileStream = File.Create(destination.FullName))
                {
                    using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        /// <summary>
        ///     A FileInfo extension method that extracts the g zip to directory described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void ExtractGZipToDirectory(this FileInfo @this)
        {
            using (FileStream originalFileStream = @this.OpenRead())
            {
                string newFileName = Path.GetFileNameWithoutExtension(@this.FullName);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }

        /// <summary>
        ///     A FileInfo extension method that extracts the g zip to directory described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destination">Destination for the.</param>
        public static void ExtractGZipToDirectory(this FileInfo @this, string destination)
        {
            using (FileStream originalFileStream = @this.OpenRead())
            {
                using (FileStream compressedFileStream = File.Create(destination))
                {
                    using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        /// <summary>
        ///     A FileInfo extension method that extracts the g zip to directory described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destination">Destination for the.</param>
        public static void ExtractGZipToDirectory(this FileInfo @this, FileInfo destination)
        {
            using (FileStream originalFileStream = @this.OpenRead())
            {
                using (FileStream compressedFileStream = File.Create(destination.FullName))
                {
                    using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
        }
        /// <summary>
        ///     A string extension method that compress the given string to GZip byte array.
        /// </summary>
        /// <param name="this">The stringToCompress to act on.</param>
        /// <returns>The string compressed into a GZip byte array.</returns>
        public static byte[] CompressGZip(this string @this)
        {
            byte[] stringAsBytes = Encoding.Default.GetBytes(@this);
            using (var memoryStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    zipStream.Write(stringAsBytes, 0, stringAsBytes.Length);
                    zipStream.Close();
                    return (memoryStream.ToArray());
                }
            }
        }

        /// <summary>
        ///     A string extension method that compress the given string to GZip byte array.
        /// </summary>
        /// <param name="this">The stringToCompress to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The string compressed into a GZip byte array.</returns>
        public static byte[] CompressGZip(this string @this, Encoding encoding)
        {
            byte[] stringAsBytes = encoding.GetBytes(@this);
            using (var memoryStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    zipStream.Write(stringAsBytes, 0, stringAsBytes.Length);
                    zipStream.Close();
                    return (memoryStream.ToArray());
                }
            }
        }


         

    }