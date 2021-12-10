using System;

public interface IReader : IBase {
    public void ReadHead(string fileName);
    public bool ReadBool();
    public sbyte ReadInt8();
    public byte ReadUInt8();
    public short ReadInt16();
    public ushort ReadUInt16();
    public int ReadInt32();
    public uint ReadUInt32();
    public long ReadInt64();
    public ulong ReadUInt64();
    public float ReadFloat();
    public double ReadDouble();
    public string ReadString();
    public DateTime ReadDateTime();
    public byte[] ReadBytes();
}
