using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace ScorpioProto.Commons {
    public static class ScorpioUtil {
        private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static bool HasSign(int sign, int index) {
            return (sign & (1 << index)) != 0;
        }
        public static int AddSign(int sign, int index) {
            if ((sign & (1 << index)) == 0)
                sign |= (1 << index);
            return sign;
        }
        public static void WriteString(BinaryWriter writer, string value) {
            if (string.IsNullOrEmpty(value)) {
                writer.Write(0);
            } else {
                var bytes = Encoding.UTF8.GetBytes(value);
                writer.Write(bytes.Length);
                writer.Write(bytes);
            }
        }
        public static string ReadString(BinaryReader reader) {
            var length = reader.ReadInt32();
            if (length <= 0) { return ""; }
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }
        public static sbyte ToInt8(object value) {
            return Convert.ToSByte(value);
        }
        public static short ToInt16(object value) {
            return Convert.ToInt16(value);
        }
        public static int ToInt32(object value) {
            return Convert.ToInt32(value);
        }
        public static long ToInt64(object value) {
            return Convert.ToInt64(value);
        }
        public static float ToFloat(object value) {
            return Convert.ToSingle(value);
        }
        public static double ToDouble(object value) {
            return Convert.ToDouble(value);
        }
        public static DateTime ToDateTime(long span) {
            DateTime startTime = BaseTime;
            return startTime.AddMilliseconds(span);
        }
        public static string ToString(IList list) {
            int count = list != null ? list.Count : 0;
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            for (int i = 0;i< count;++i) {
                builder.Append(list[i]);
                if (i != count - 1) {
                    builder.Append(",");
                }
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}
