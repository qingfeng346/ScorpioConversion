using System;
using System.IO;
using System.Text;
namespace Scorpio.Conversion {
    public class DefaultReader : IReader {
        private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        BinaryReader reader;
        Stream stream;
        bool closeStream;
        public DefaultReader(Stream stream, bool closeStream = false) {
            this.stream = stream;
            this.reader = new BinaryReader(stream);
            this.closeStream = closeStream;
        }
        public void ReadHead() {
            ConversionUtil.ReadHead(this);
        }
        public bool ReadBool() {
            return ReadInt8() == 1;
        }
        public sbyte ReadInt8() {
            return reader.ReadSByte();
        }
        public byte ReadUInt8() {
            return reader.ReadByte();
        }
        public short ReadInt16() {
            return reader.ReadInt16();
        }
        public ushort ReadUInt16() {
            return reader.ReadUInt16();
        }
        public int ReadInt32() {
            return reader.ReadInt32();
        }
        public uint ReadUInt32() {
            return reader.ReadUInt32();
        }
        public long ReadInt64() {
            return reader.ReadInt64();
        }
        public ulong ReadUInt64() {
            return reader.ReadUInt64();
        }
        public float ReadFloat() {
            return reader.ReadSingle();
        }
        public double ReadDouble() {
            return reader.ReadDouble();
        }
        public string ReadString() {
            var length = ReadUInt16();
            if (length == 0) return "";
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }
        public DateTime ReadDateTime() {
            DateTime startTime = BaseTime;
            return startTime.AddMilliseconds(reader.ReadInt64());
        }
        public byte[] ReadBytes() {
            var length = ReadInt32();
            return reader.ReadBytes(length);
        }
        public void Close() {
            reader.Dispose();
            if (closeStream) {
                stream.Dispose();
            }
        }
        public void Dispose() {
            Close();
        }
    }
}
