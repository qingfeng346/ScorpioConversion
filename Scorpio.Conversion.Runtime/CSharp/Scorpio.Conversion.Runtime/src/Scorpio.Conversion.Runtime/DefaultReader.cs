using System;
using System.IO;
using System.Text;
namespace Scorpio.Conversion.Runtime {
    public class DefaultReader : IReader {
        private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        BinaryReader reader;
        Stream stream;
        bool closeStream;
        public DefaultReader(string file) : this(File.OpenRead(file), true) { }
        public DefaultReader(byte[] buffer) : this(new MemoryStream(buffer), true) { }
        public DefaultReader(Stream stream, bool closeStream = false) {
            this.stream = stream;
            this.reader = new BinaryReader(stream);
            this.closeStream = closeStream;
        }
        public virtual void ReadHead() {
            ConversionUtil.ReadHead(this);
        }
        public virtual bool ReadBool() {
            return ReadInt8() == 1;
        }
        public virtual sbyte ReadInt8() {
            return reader.ReadSByte();
        }
        public virtual byte ReadUInt8() {
            return reader.ReadByte();
        }
        public virtual short ReadInt16() {
            return reader.ReadInt16();
        }
        public virtual ushort ReadUInt16() {
            return reader.ReadUInt16();
        }
        public virtual int ReadInt32() {
            return reader.ReadInt32();
        }
        public virtual uint ReadUInt32() {
            return reader.ReadUInt32();
        }
        public virtual long ReadInt64() {
            return reader.ReadInt64();
        }
        public virtual ulong ReadUInt64() {
            return reader.ReadUInt64();
        }
        public virtual float ReadFloat() {
            return reader.ReadSingle();
        }
        public virtual double ReadDouble() {
            return reader.ReadDouble();
        }
        public virtual string ReadString() {
            var length = ReadUInt16();
            if (length == 0) return "";
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }
        public virtual string ReadL10n(string key) {
            ReadString();
            return key;
        }
        public virtual DateTime ReadDateTime() {
            return BaseTime.AddMilliseconds(reader.ReadInt64());
        }
        public virtual byte[] ReadBytes() {
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
