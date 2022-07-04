using System;
using System.IO;

namespace Scorpio.Conversion.Engine {
    public class ScriptReader : IReader, ScriptBase {
        public ScriptValue Value { get; set; }
        public ScriptReader(ScriptValue value, object[] args) {
            Value = value.call(ScriptValue.Null, args);
        }

        public void Initialize(byte[] buffer) {
            this.Call("Initialize", buffer);
        }

        public void Initialize(Stream stream, bool closeStream = false) {
            this.Call("Initialize", stream, closeStream);
        }

        public bool ReadBool() {
            return this.Call("ReadBool").IsTrue;
        }

        public byte[] ReadBytes() {
            return (byte[])this.Call("ReadBytes").Value;
        }

        public DateTime ReadDateTime() {
            return (DateTime)this.Call("ReadDateTime").Value;
        }

        public double ReadDouble() {
            return this.Call("ReadDouble").ToDouble();
        }

        public float ReadFloat() {
            return (float)this.Call("ReadDouble").ToDouble();
        }

        public short ReadInt16() {
            return (short)this.Call("ReadInt16").ToLong();
        }

        public int ReadInt32() {
            return this.Call("ReadInt16").ToInt32();
        }

        public long ReadInt64() {
            return this.Call("ReadInt16").ToLong();
        }

        public sbyte ReadInt8() {
            return (sbyte)this.Call("ReadInt16").ToLong();
        }

        public string ReadL10N(string key) {
            return this.Call("ReadL10N", key).ToString();
        }

        public string ReadString() {
            return this.Call("ReadString").ToString();
        }

        public ushort ReadUInt16() {
            return (ushort)this.Call("ReadUInt16").ToLong();
        }

        public uint ReadUInt32() {
            return (uint)this.Call("ReadUInt32").ToLong();
        }

        public ulong ReadUInt64() {
            var ret = this.Call("ReadUInt64");
            switch (ret.valueType) {
                case ScriptValue.doubleValueType: return (ulong)ret.doubleValue;
                case ScriptValue.longValueType: return (ulong)ret.longValue;
                case ScriptValue.objectValueType: return Convert.ToUInt64(ret.objectValue);
                default: throw new System.Exception($"类型[{ret.ValueTypeName}]不支持转换为 ulong");
            }
        }

        public byte ReadUInt8() {
            return (byte)this.Call("ReadUInt8").ToLong();
        }

        public void Dispose() { }
    }
}
