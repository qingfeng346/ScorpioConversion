using System;
using System.IO;
using System.Text;
public class TableWriter
{
    MemoryStream stream = new MemoryStream();
    BinaryWriter writer;
    public TableWriter()
    {
        writer = new BinaryWriter(stream);
    }
    public void Seek(int offset)
    {
        writer.Seek(offset, SeekOrigin.Begin);
    }
    public void WriteBool(bool value)
    {
        writer.Write(value ? (sbyte)1 : (sbyte)0);
    }
    public void WriteByte(sbyte value)
    {
        writer.Write(value);
    }
    public void WriteInt32(int value)
    {
        writer.Write(value);
    }
    public void WriteBool(string value)
    {
        writer.Write((Util.IsEmptyString(value) ? Util.INVALID_BOOL : Util.ToBoolean(value, false)) ? (sbyte)1 : (sbyte)0);
    }
    public void WriteInt8(string value)
    {
        writer.Write(Util.IsEmptyString(value) ? Util.INVALID_INT8 : Convert.ToSByte(value));
    }
    public void WriteInt16(string value)
    {
        writer.Write(Util.IsEmptyString(value) ? Util.INVALID_INT16 : Convert.ToInt16(value));
    }
    public void WriteInt32(string value)
    {
        writer.Write(Util.IsEmptyString(value) ? Util.INVALID_INT32 : Convert.ToInt32(value));
    }
    public void WriteInt64(string value)
    {
        writer.Write(Util.IsEmptyString(value) ? Util.INVALID_INT64 : Convert.ToInt64(value));
    }
    public void WriteFloat(string value)
    {
        writer.Write(Util.IsEmptyString(value) ? Util.INVALID_FLOAT : Convert.ToSingle(value));
    }
    public void WriteDouble(string value)
    {
        writer.Write(Util.IsEmptyString(value) ? Util.INVALID_DOUBLE : Convert.ToDouble(value));
    }
    public void WriteString(string value)
    {
        if (Util.IsEmptyString(value)) {
            writer.Write((byte)0);
        } else {
            writer.Write(Encoding.UTF8.GetBytes(value));
            writer.Write((byte)0);
        }
    }
    public byte[] ToArray()
    {
        stream.Position = 0;
        return stream.ToArray();
    }
}

