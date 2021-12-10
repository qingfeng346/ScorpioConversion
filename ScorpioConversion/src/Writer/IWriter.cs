using System;
public interface IWriter : IBase {
    public void WriteHead(TableBuilder builder);
    public void WriteBool(bool value);
    public void WriteInt8(sbyte value);
    public void WriteUInt8(byte value);
    public void WriteInt16(short value);
    public void WriteUInt16(ushort value);
    public void WriteInt32(int value);
    public void WriteUInt32(uint value);
    public void WriteInt64(long value);
    public void WriteUInt64(ulong value);
    public void WriteFloat(float value);
    public void WriteDouble(double value);
    public void WriteString(string value);
    public void WriteDateTime(DateTime value);
    public void WriteBytes(byte[] value);
    public byte[] ToArray();
}
