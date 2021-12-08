using System;
namespace ScorpioProto.Commons {
    public interface IScorpioReader : IDisposable {
        int ReadHead(string fileName, string MD5);
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
    }
}