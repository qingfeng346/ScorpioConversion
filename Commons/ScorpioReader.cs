using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
namespace Scorpio.Commons
{
    public class ScorpioReader
    {
        MemoryStream stream;
        BinaryReader reader;
        public ScorpioReader(byte[] buffer)
        {
            stream = new MemoryStream(buffer);
            reader = new BinaryReader(stream);
        }
        public bool ReadBool()
        {
            return ReadInt8() == (sbyte)1;
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
            return ScorpioUtil.ReadString(reader);
        }
        public byte[] ReadBytes()
        {
            int length = reader.ReadInt32();
            return reader.ReadBytes(length);
        }
        public void Close()
        {
            stream.Dispose();
#if SCORPIO_UWP && !UNITY_EDITOR
            reader.Dispose();
#else
            reader.Close();
#endif
        }
    }
}
