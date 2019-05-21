using System;
namespace ScorpioProto.Commons {
    public interface IScorpioReader {
        bool ReadBool();
        sbyte ReadInt8();
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        float ReadFloat();
        double ReadDouble();
        string ReadString();
        DateTime ReadDateTime();
        byte[] ReadBytes();
    }
}