using System;
using System.Text;
using System.IO;

public class DefaultReader : IReader, IDisposable {
    private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    MemoryStream stream;
    BinaryReader reader;
    public string Name => "default";
    public DefaultReader() {
        
    }
    public void Initialize(byte[] buffer) {
        stream = new MemoryStream(buffer);
        reader = new BinaryReader(stream);
    }
    public void ReadHead(string fileName) {
        {
            var number = ReadInt32();        //字段数量
            for (var i = 0; i < number; ++i) {
                if (ReadInt8() == 0) {   //基础类型
                    ReadInt8();          //基础类型索引
                } else {                        //自定义类
                    ReadString();        //自定义类名称
                }
                ReadBool();          //是否是数组
            }
        }
        {
            var customNumber = ReadInt32();  //自定义类数量
            for (var i = 0; i < customNumber; ++i) {
                ReadString();                //读取自定义类名字
                var number = ReadInt32();        //字段数量
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