using System;
using System.Text;
using System.IO;

namespace Scorpio.Conversion.Engine {
    [AutoReader("default")]
    public class DefaultReader : IReader, IDisposable {
        private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        BinaryReader reader;
        Stream stream;
        bool closeStream;
        public void Initialize(byte[] buffer) {
            Initialize(new MemoryStream(buffer), true);
        }
        public void Initialize(Stream stream, bool closeStream = false) {
            this.stream = stream;
            this.reader = new BinaryReader(stream);
            this.closeStream = closeStream;
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
            var length = reader.ReadUInt16();
            if (length <= 0) return "";
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }
        public string ReadL10N(string key) {
            return ReadString();
        }
        public DateTime ReadDateTime() {
            DateTime startTime = BaseTime;
            return startTime.AddMilliseconds(reader.ReadInt64());
        }
        public byte[] ReadBytes() {
            var length = reader.ReadInt32();
            return reader.ReadBytes(length);
        }
        public void Dispose() {
            if (reader != null) {
                reader.Close();
                reader = null;
            }
            if (closeStream) {
                stream.Dispose();
            }
        }
    }
}