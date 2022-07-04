using System;
namespace Scorpio.Conversion.Engine {
    public class ScriptWriter : IWriter, ScriptBase {
        public ScriptValue Value { get; set; }
        public ScriptWriter(ScriptValue value, object[] args) {
            Value = value.call(ScriptValue.Null, args);
        }
        public void Dispose() { }
        public byte[] ToArray() {
            return (byte[])this.Call("ToArray").Value;
        }
        public void WriteBool(bool value) {
            this.Call("WriteBool", value);
        }
        public void WriteBytes(byte[] value) {
            this.Call("WriteBytes", value);
        }
        public void WriteDateTime(DateTime value) {
            this.Call("WriteDateTime", value);
        }
        public void WriteDouble(double value) {
            this.Call("WriteDouble", value);
        }
        public void WriteFloat(float value) {
            this.Call("WriteFloat", value);
        }
        public void WriteInt16(short value) {
            this.Call("WriteInt16", value);
        }
        public void WriteInt32(int value) {
            this.Call("WriteInt32", value);
        }
        public void WriteInt64(long value) {
            this.Call("WriteInt64", value);
        }
        public void WriteInt8(sbyte value) {
            this.Call("WriteInt8", value);
        }
        public void WriteString(string value) {
            this.Call("WriteString", value);
        }
        public void WriteUInt16(ushort value) {
            this.Call("WriteUInt16", value);
        }
        public void WriteUInt32(uint value) {
            this.Call("WriteUInt32", value);
        }
        public void WriteUInt64(ulong value) {
            this.Call("WriteUInt64", value);
        }
        public void WriteUInt8(byte value) {
            this.Call("WriteUInt8", value);
        }
    }
}
