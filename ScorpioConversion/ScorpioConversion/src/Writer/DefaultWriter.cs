using System;
using System.IO;
using System.Text;

[AutoWriter("default")]
public class DefaultWriter : IWriter, IDisposable {
    MemoryStream stream;
    BinaryWriter writer;
    public DefaultWriter() {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);
    }
    public void WriteBool(bool value) {
        writer.Write(value ? (sbyte)1 : (sbyte)0);
    }
    public void WriteInt8(sbyte value) {
        writer.Write(value);
    }
    public void WriteUInt8(byte value) {
        writer.Write(value);
    }
    public void WriteInt16(short value) {
        writer.Write(value);
    }
    public void WriteUInt16(ushort value) {
        writer.Write(value);
    }
    public void WriteInt32(int value) {
        writer.Write(value);
    }
    public void WriteUInt32(uint value) {
        writer.Write(value);
    }
    public void WriteInt64(long value) {
        writer.Write(value);
    }
    public void WriteUInt64(ulong value) {
        writer.Write(value);
    }
    public void WriteFloat(float value) {
        writer.Write(value);
    }
    public void WriteDouble(double value) {
        writer.Write(value);
    }
    public void WriteString(string value) {
        if (value.IsEmptyString()) {
            writer.Write((ushort)0);
        } else {
            var bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(Convert.ToUInt16(bytes.Length));
            writer.Write(bytes);
        }
    }
    public void WriteDateTime(DateTime value) {
        writer.Write(BasicUtil.GetTimeSpan(value));
    }
    public void WriteBytes(byte[] value) {
        if (value == null) {
            writer.Write((int)0);
        } else {
            writer.Write(value.Length);
            if (value.Length > 0) {
                writer.Write(value);
            }
        }
    }
    public byte[] ToArray() {
        stream.Position = 0;
        return stream.ToArray();
    }
    public void Dispose() {
        stream.Dispose();
        writer.Dispose();
    }
}