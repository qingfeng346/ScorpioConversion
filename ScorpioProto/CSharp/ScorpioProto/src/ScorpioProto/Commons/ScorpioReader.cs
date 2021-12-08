using System;
using System.IO;
using System.Text;
namespace ScorpioProto.Commons {
    public class ScorpioReader : IScorpioReader {
        private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        MemoryStream stream;
        BinaryReader reader;
        public ScorpioReader(byte[] buffer) {
            stream = new MemoryStream(buffer);
            reader = new BinaryReader(stream);
        }
        public int ReadHead(string fileName, string MD5) {
            int iRow = reader.ReadInt32();          //行数
            if (reader.ReadString() != MD5)         //验证文件MD5(检测结构是否改变)
                throw new Exception($"文件[{fileName}]版本验证失败");
            {
                var number = reader.ReadInt32();        //字段数量
                for (var i = 0; i < number; ++i) {
                    if (ReadInt8() == 0) {   //基础类型
                        ReadInt8();          //基础类型索引
                    } else {                        //自定义类
                        reader.ReadString();        //自定义类名称
                    }
                    ReadBool();          //是否是数组
                }
            }
            {
                var customNumber = reader.ReadInt32();  //自定义类数量
                for (var i = 0; i < customNumber; ++i) {
                    reader.ReadString();                //读取自定义类名字
                    var number = reader.ReadInt32();        //字段数量
                    for (var j = 0; j < number; ++j) {
                        if (ReadInt8() == 0) {   //基础类型
                            ReadInt8();          //基础类型索引
                        } else {                        //自定义类
                            ReadString();        //自定义类名称
                        }
                        ReadBool();          //是否是数组
                    }
                }
            }
            return iRow;
        }
        public bool ReadBool() {
            return ReadInt8() == 1;
        }
        public sbyte ReadInt8() {
            return reader.ReadSByte();
        }
        public byte ReadUInt8() {
            return reader.ReadByte();
        }
        public short ReadInt16() {
            return reader.ReadInt16();
        }
        public ushort ReadUInt16() {
            return reader.ReadUInt16();
        }
        public int ReadInt32() {
            return reader.ReadInt32();
        }
        public uint ReadUInt32() {
            return reader.ReadUInt32();
        }
        public long ReadInt64() {
            return reader.ReadInt64();
        }
        public ulong ReadUInt64() {
            return reader.ReadUInt64();
        }
        public float ReadFloat() {
            return reader.ReadSingle();
        }
        public double ReadDouble() {
            return reader.ReadDouble();
        }
        public string ReadString() {
            var length = reader.ReadUInt16();
            if (length <= 0) return "";
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }
        public DateTime ReadDateTime() {
            DateTime startTime = BaseTime;
            return startTime.AddMilliseconds(reader.ReadInt64());
        }
        public byte[] ReadBytes() {
            var length = reader.ReadInt32();
            return reader.ReadBytes(length);
        }
        public void Dispose() {
            stream.Close();
            reader.Close();
        }
    }
}
