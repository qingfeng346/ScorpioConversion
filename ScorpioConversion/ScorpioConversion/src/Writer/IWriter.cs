using System;
public interface IWriter {
    void WriteHead(object builder);
    void WriteBool(bool value);
    void WriteInt8(sbyte value);
    void WriteUInt8(byte value);
    void WriteInt16(short value);
    void WriteUInt16(ushort value);
    void WriteInt32(int value);
    void WriteUInt32(uint value);
    void WriteInt64(long value);
    void WriteUInt64(ulong value);
    void WriteFloat(float value);
    void WriteDouble(double value);
    void WriteString(string value);
    void WriteDateTime(DateTime value);
    void WriteBytes(byte[] value);
    byte[] ToArray();
}
