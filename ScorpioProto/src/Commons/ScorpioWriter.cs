using System;
using System.IO;
using System.Text;
namespace Scorpio.Commons
{
    public class ScorpioWriter
    {
        MemoryStream stream;
        BinaryWriter writer;
        public ScorpioWriter()
        {
            stream = new MemoryStream();
            writer = new BinaryWriter(stream);
        }
        public void WriteBool(bool value)
        {
            writer.Write(value ? (sbyte)1 : (sbyte)0);
        }
        public void WriteInt8(sbyte value)
        {
            writer.Write(value);
        }
        public void WriteInt16(short value)
        {
            writer.Write(value);
        }
        public void WriteInt32(int value)
        {
            writer.Write(value);
        }
        public void WriteInt64(long value)
        {
            writer.Write(value);
        }
        public void WriteFloat(float value)
        {
            writer.Write(value);
        }
        public void WriteDouble(double value)
        {
            writer.Write(value);
        }
        public void WriteString(string value)
        {
            ScorpioUtil.WriteString(writer, value);
        }
        public void WriteBytes(byte[] value)
        {
            writer.Write((int)value.Length);
            writer.Write(value);
        }
        public byte[] ToArray()
        {
            stream.Position = 0;
            return stream.ToArray();
        }
    }
}

