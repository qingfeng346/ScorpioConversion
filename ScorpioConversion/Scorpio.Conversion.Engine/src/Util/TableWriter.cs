using System;
using System.Collections.Generic;

namespace Scorpio.Conversion {
    public class TableWriter : IDisposable {
        IWriter writer = null;
        public TableWriter(IWriter writer) {
            this.writer = writer;
        }
        public void WriteInt32(int value) {
            writer.WriteInt32(value);
        }
        public void WriteBytes(byte[] bytes) {
            writer.WriteBytes(bytes);
        }
        public void WriteHead(PackageClass packageClass, HashSet<IPackage> customTypes) {
            writer.WriteHead(packageClass, customTypes);
        }
        public void WriteBool(string value) {
            writer.WriteBool(value.ToBoolean());
        }
        public void WriteInt8(string value) {
            writer.WriteInt8(value.ToInt8());
        }
        public void WriteUInt8(string value) {
            writer.WriteUInt8(value.ToUInt8());
        }
        public void WriteInt16(string value) {
            writer.WriteInt16(value.ToInt16());
        }
        public void WriteUInt16(string value) {
            writer.WriteUInt16(value.ToUInt16());
        }
        public void WriteInt32(string value) {
            writer.WriteInt32(value.ToInt32());
        }
        public void WriteUInt32(string value) {
            writer.WriteUInt32(value.ToUInt32());
        }
        public void WriteInt64(string value) {
            writer.WriteInt64(value.ToInt64());
        }
        public void WriteUInt64(string value) {
            writer.WriteUInt64(value.ToUInt64());
        }
        public void WriteFloat(string value) {
            writer.WriteFloat(value.ToFloat());
        }
        public void WriteDouble(string value) {
            writer.WriteDouble(value.ToDouble());
        }
        public void WriteString(string value) {
            writer.WriteString(value);
        }
        public void WriteDateTime(string value) {
            writer.WriteDateTime(value.ToDateTime());
        }
        public void WriteBytes(string value) {
            writer.WriteBytes(value.ToBytes());
        }
        public byte[] ToArray() {
            return writer.ToArray();
        }
        public void Dispose() {
            writer.Dispose();
        }
    }
}