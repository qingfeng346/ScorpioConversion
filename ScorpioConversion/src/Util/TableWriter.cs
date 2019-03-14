using System;
using System.IO;
using System.Text;
public class TableWriter :IDisposable {
    MemoryStream stream;
    BinaryWriter writer;
    public TableWriter() {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);
    }
    public void Seek(int offset) {
        writer.Seek(offset, SeekOrigin.Begin);
    }
    public void WriteBool(bool value) {
        writer.Write(value ? (sbyte)1 : (sbyte)0);
    }
    public void WriteInt8(sbyte value) {
        writer.Write(value);
    }
    public void WriteInt32(int value) {
        writer.Write(value);
    }
    public void WriteBool(string value) {
        writer.Write(value.ToBoolean() ? (sbyte)1 : (sbyte)0);
    }
    public void WriteInt8(string value) {
        writer.Write(value.ToInt8());
    }
    public void WriteInt16(string value) {
        writer.Write(value.ToInt16());
    }
    public void WriteInt32(string value) {
        writer.Write(value.ToInt32());
    }
    public void WriteInt64(string value) {
        writer.Write(value.ToInt64());
    }
    public void WriteFloat(string value) {
        writer.Write(value.ToFloat());
    }
    public void WriteDouble(string value) {
        writer.Write(value.ToDouble());
    }
    public void WriteString(string value) {
        if (value.IsEmptyString()) {
            writer.Write((int)0);
        } else {
            var bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }
    }
    public void WriteDateTime(string value) {
        writer.Write(BasicUtil.GetTimeSpan(DateTime.FromOADate(double.Parse(value))));
    }
    public byte[] ToArray() {
        stream.Position = 0;
        return stream.ToArray();
    }
    public void Dispose() {
        stream.Close();
        writer.Close();
    }
}

