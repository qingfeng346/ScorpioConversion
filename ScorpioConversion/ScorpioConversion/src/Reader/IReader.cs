using System;
using System.IO;
namespace Scorpio.Conversion {
    public interface IReader : IDisposable {
        void Initialize(byte[] buffer);
        void Initialize(Stream stream);
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
        string ReadL10N(string key);
        DateTime ReadDateTime();
        byte[] ReadBytes();
    }
}