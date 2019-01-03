using System;
using System.Data;
using System.IO;

public static class ByteExtensions
    {
 
        public static SqlDbType SqlSystemTypeToSqlDbType(this byte @this)
        {
            switch (@this)
            {
                case 34: // 34 | "image" | SqlDbType.Image
                    return SqlDbType.Image;

                case 35: // 35 | "text" | SqlDbType.Text
                    return SqlDbType.Text;

                case 36: // 36 | "uniqueidentifier" | SqlDbType.UniqueIdentifier
                    return SqlDbType.UniqueIdentifier;

                case 40: // 40 | "date" | SqlDbType.Date
                    return SqlDbType.Date;

                case 41: // 41 | "time" | SqlDbType.Time
                    return SqlDbType.Time;

                case 42: // 42 | "datetime2" | SqlDbType.DateTime2
                    return SqlDbType.DateTime2;

                case 43: // 43 | "datetimeoffset" | SqlDbType.DateTimeOffset
                    return SqlDbType.DateTimeOffset;

                case 48: // 48 | "tinyint" | SqlDbType.TinyInt
                    return SqlDbType.TinyInt;

                case 52: // 52 | "smallint" | SqlDbType.SmallInt
                    return SqlDbType.SmallInt;

                case 56: // 56 | "int" | SqlDbType.Int
                    return SqlDbType.Int;

                case 58: // 58 | "smalldatetime" | SqlDbType.SmallDateTime
                    return SqlDbType.SmallDateTime;

                case 59: // 59 | "real" | SqlDbType.Real
                    return SqlDbType.Real;

                case 60: // 60 | "money" | SqlDbType.Money
                    return SqlDbType.Money;

                case 61: // 61 | "datetime" | SqlDbType.DateTime
                    return SqlDbType.DateTime;

                case 62: // 62 | "float" | SqlDbType.Float
                    return SqlDbType.Float;

                case 98: // 98 | "sql_variant" | SqlDbType.Variant
                    return SqlDbType.Variant;

                case 99: // 99 | "ntext" | SqlDbType.NText
                    return SqlDbType.NText;

                case 104: // 104 | "bit" | SqlDbType.Bit
                    return SqlDbType.Bit;

                case 106: // 106 | "decimal" | SqlDbType.Decimal
                    return SqlDbType.Decimal;

                case 108: // 108 | "numeric" | SqlDbType.Decimal
                    return SqlDbType.Decimal;

                case 122: // 122 | "smallmoney" | SqlDbType.SmallMoney
                    return SqlDbType.SmallMoney;

                case 127: // 127 | "bigint" | SqlDbType.BigInt
                    return SqlDbType.BigInt;

                case 165: // 165 | "varbinary" | SqlDbType.VarBinary
                    return SqlDbType.VarBinary;

                case 167: // 167 | "varchar" | SqlDbType.VarChar
                    return SqlDbType.VarChar;

                case 173: // 173 | "binary" | SqlDbType.Binary
                    return SqlDbType.Binary;

                case 175: // 175 | "char" | SqlDbType.Char
                    return SqlDbType.Char;

                case 189: // 189 | "timestamp" | SqlDbType.Timestamp
                    return SqlDbType.Timestamp;

                case 231: // 231 | "nvarchar", "sysname" | SqlDbType.NVarChar
                    return SqlDbType.NVarChar;

                case 239: // 239 | "nchar" | SqlDbType.NChar
                    return SqlDbType.NChar;

                case 240: // 240 | "hierarchyid", "geometry", "geography" | SqlDbType.Udt
                    return SqlDbType.Udt;

                case 241: // 241 | "xml" | SqlDbType.Xml
                    return SqlDbType.Xml;

                default:
                    throw new Exception(string.Format("Unsupported Type: {0}. Please let us know about this type and we will support it: sales@zzzprojects.com", @this));
            }
        }

        /// <summary>
        ///     Returns the larger of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        /// <returns>Parameter  or , whichever is larger.</returns>
        public static Byte Max(this Byte val1, Byte val2)
        {
            return Math.Max(val1, val2);
        }
        /// <summary>
        ///     Returns the smaller of two 8-bit unsigned integers.
        /// </summary>
        /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
        /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
        /// <returns>Parameter  or , whichever is smaller.</returns>
        public static Byte Min(this Byte val1, Byte val2)
        {
            return Math.Min(val1, val2);
        }


    }


    public class BitStream : Stream
    {
        private byte[] Source { get; set; }

        /// <summary>
        /// Initialize the stream with capacity
        /// </summary>
        /// <param name="capacity">Capacity of the stream</param>
        public BitStream(int capacity)
        {
            this.Source = new byte[capacity];
        }

        /// <summary>
        /// Initialize the stream with a source byte array
        /// </summary>
        /// <param name="source"></param>
        public BitStream(byte[] source)
        {
            this.Source = source;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bit length of the stream
        /// </summary>
        public override long Length
        {
            get { return Source.Length * 8; }
        }

        /// <summary>
        /// Bit position of the stream
        /// </summary>
        public override long Position { get; set; }

        /// <summary>
        /// Read the stream to the buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="offset">Offset bit start position of the stream</param>
        /// <param name="count">Number of bits to read</param>
        /// <returns>Number of bits read</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            // Temporary position cursor
            long tempPos = this.Position;
            tempPos += offset;

            // Buffer byte position and in-byte position
            int readPosCount = 0, readPosMod = 0;

            // Stream byte position and in-byte position
            long posCount = tempPos >> 3;
            int posMod = (int)(tempPos - ((tempPos >> 3) << 3));

            while (tempPos < this.Position + offset + count && tempPos < this.Length)
            {
                // Copy the bit from the stream to buffer
                if ((((int)this.Source[posCount]) & (0x1 << (7 - posMod))) != 0)
                {
                    buffer[readPosCount] = (byte)((int)(buffer[readPosCount]) | (0x1 << (7 - readPosMod)));
                }
                else
                {
                    buffer[readPosCount] = (byte)((int)(buffer[readPosCount]) & (0xffffffff - (0x1 << (7 - readPosMod))));
                }

                // Increment position cursors
                tempPos++;
                if (posMod == 7)
                {
                    posMod = 0;
                    posCount++;
                }
                else
                {
                    posMod++;
                }
                if (readPosMod == 7)
                {
                    readPosMod = 0;
                    readPosCount++;
                }
                else
                {
                    readPosMod++;
                }
            }
            int bits = (int)(tempPos - this.Position - offset);
            this.Position = tempPos;
            return bits;
        }

        /// <summary>
        /// Set up the stream position
        /// </summary>
        /// <param name="offset">Position</param>
        /// <param name="origin">Position origin</param>
        /// <returns>Position after setup</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case (SeekOrigin.Begin):
                    {
                        this.Position = offset;
                        break;
                    }
                case (SeekOrigin.Current):
                    {
                        this.Position += offset;
                        break;
                    }
                case (SeekOrigin.End):
                    {
                        this.Position = this.Length + offset;
                        break;
                    }
            }
            return this.Position;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write from buffer to the stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset">Offset start bit position of buffer</param>
        /// <param name="count">Number of bits</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            // Temporary position cursor
            long tempPos = this.Position;

            // Buffer byte position and in-byte position
            int readPosCount = offset >> 3, readPosMod = offset - ((offset >> 3) << 3);

            // Stream byte position and in-byte position
            long posCount = tempPos >> 3;
            int posMod = (int)(tempPos - ((tempPos >> 3) << 3));

            while (tempPos < this.Position + count && tempPos < this.Length)
            {
                // Copy the bit from buffer to the stream
                if ((((int)buffer[readPosCount]) & (0x1 << (7 - readPosMod))) != 0)
                {
                    this.Source[posCount] = (byte)((int)(this.Source[posCount]) | (0x1 << (7 - posMod)));
                }
                else
                {
                    this.Source[posCount] = (byte)((int)(this.Source[posCount]) & (0xffffffff - (0x1 << (7 - posMod))));
                }

                // Increment position cursors
                tempPos++;
                if (posMod == 7)
                {
                    posMod = 0;
                    posCount++;
                }
                else
                {
                    posMod++;
                }
                if (readPosMod == 7)
                {
                    readPosMod = 0;
                    readPosCount++;
                }
                else
                {
                    readPosMod++;
                }
            }
            this.Position = tempPos;
        }
    }