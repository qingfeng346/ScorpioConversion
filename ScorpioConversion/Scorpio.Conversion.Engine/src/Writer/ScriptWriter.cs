using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Conversion.Engine {
    public class ScriptWriter : IWriter {
        public ScriptValue Value { get; private set; }
        public ScriptWriter(ScriptValue value, object[] args) {
            Value = value.call(ScriptValue.Null, args);
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        public byte[] ToArray() {
            throw new NotImplementedException();
        }

        public void WriteBool(bool value) {
            throw new NotImplementedException();
        }

        public void WriteBytes(byte[] value) {
            throw new NotImplementedException();
        }

        public void WriteDateTime(DateTime value) {
            throw new NotImplementedException();
        }

        public void WriteDouble(double value) {
            throw new NotImplementedException();
        }

        public void WriteFloat(float value) {
            throw new NotImplementedException();
        }

        public void WriteInt16(short value) {
            throw new NotImplementedException();
        }

        public void WriteInt32(int value) {
            throw new NotImplementedException();
        }

        public void WriteInt64(long value) {
            throw new NotImplementedException();
        }

        public void WriteInt8(sbyte value) {
            throw new NotImplementedException();
        }

        public void WriteString(string value) {
            throw new NotImplementedException();
        }

        public void WriteUInt16(ushort value) {
            throw new NotImplementedException();
        }

        public void WriteUInt32(uint value) {
            throw new NotImplementedException();
        }

        public void WriteUInt64(ulong value) {
            throw new NotImplementedException();
        }

        public void WriteUInt8(byte value) {
            throw new NotImplementedException();
        }
    }
}
