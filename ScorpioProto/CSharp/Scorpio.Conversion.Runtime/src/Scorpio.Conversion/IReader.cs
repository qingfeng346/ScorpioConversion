using System;
namespace Scorpio.Conversion.Runtime {
    public interface IReader : IDisposable {
        bool ReadBool();
        sbyte ReadInt8();
        byte ReadUInt8();
        short ReadInt16();
        ushort ReadUInt16();
        int ReadInt32();
        uint ReadUInt32();
        long ReadInt64();
        ulong ReadUInt64();
        float ReadFloat();
        double ReadDouble();
        string ReadString();
        DateTime ReadDateTime();
        byte[] ReadBytes();
        void Close();
    }
}