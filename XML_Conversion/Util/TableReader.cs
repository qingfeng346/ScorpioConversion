using System;
using System.IO;
public class TableReader
{
    MemoryStream stream;
    BinaryReader reader;
    public TableReader(byte[] buffer)
    {
        stream = new MemoryStream(buffer);
        reader = new BinaryReader(stream);
    }
    public bool ReadBool()
    {
        return ReadInt8() == 1;
    }
    public sbyte ReadInt8()
    {
        return reader.ReadSByte();
    }
    public short ReadInt16()
    {
        return reader.ReadInt16();
    }
    public int ReadInt32()
    {
        return reader.ReadInt32();
    }
    public long ReadInt64()
    {
        return reader.ReadInt64();
    }
    public float ReadFloat()
    {
        return reader.ReadSingle();
    }
    public double ReadDouble()
    {
        return reader.ReadDouble();
    }
    public String ReadString()
    {
        return Util.ReadString(reader);
    }
    public void Close()
    {
        stream.Close();
        reader.Close();
    }
}

