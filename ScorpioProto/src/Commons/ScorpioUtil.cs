using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
namespace Scorpio.Commons
{
    public static class ScorpioUtil
    {
        public static bool HasSign(int sign, int index)
        {
            return (sign & (1 << index)) != 0;
        }
        public static int AddSign(int sign, int index)
        {
            if ((sign & (1 << index)) == 0)
                sign |= (1 << index);
            return sign;
        }
        public static void WriteString(BinaryWriter writer, string value)
        {
            if (string.IsNullOrEmpty(value)) {
                writer.Write((byte)0);
            } else {
                writer.Write(Encoding.UTF8.GetBytes(value));
                writer.Write((byte)0);
            }
        }
        public static string ReadString(BinaryReader reader)
        {
            List<byte> sb = new List<byte>();
            byte ch;
            while ((ch = reader.ReadByte()) != 0)
                sb.Add(ch);
            return Encoding.UTF8.GetString(sb.ToArray(), 0, sb.Count);
        }
        public static sbyte ToInt8(object value)
        {
            return Convert.ToSByte(value);
        }
        public static short ToInt16(object value)
        {
            return Convert.ToInt16(value);
        }
        public static int ToInt32(object value)
        {
            return Convert.ToInt32(value);
        }
        public static long ToInt64(object value)
        {
            return Convert.ToInt64(value);
        }
        public static float ToFloat(object value)
        {
            return Convert.ToSingle(value);
        }
        public static double ToDouble(object value)
        {
            return Convert.ToDouble(value);
        }
    }
}
