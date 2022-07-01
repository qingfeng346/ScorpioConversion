using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Scorpio.Conversion.Engine {
    public class ScriptReader : IReader {
        public ScriptValue Value { get; private set; }
        public ScriptReader(ScriptValue value, object[] args) {
            Value = value.call(ScriptValue.Null, args);
        }

        public void Initialize(byte[] buffer) {
            throw new NotImplementedException();
        }

        public void Initialize(Stream stream, bool closeStream = false) {
            throw new NotImplementedException();
        }

        public bool ReadBool() {
            throw new NotImplementedException();
        }

        public byte[] ReadBytes() {
            throw new NotImplementedException();
        }

        public DateTime ReadDateTime() {
            throw new NotImplementedException();
        }

        public double ReadDouble() {
            throw new NotImplementedException();
        }

        public float ReadFloat() {
            throw new NotImplementedException();
        }

        public short ReadInt16() {
            throw new NotImplementedException();
        }

        public int ReadInt32() {
            throw new NotImplementedException();
        }

        public long ReadInt64() {
            throw new NotImplementedException();
        }

        public sbyte ReadInt8() {
            throw new NotImplementedException();
        }

        public string ReadL10N(string key) {
            throw new NotImplementedException();
        }

        public string ReadString() {
            throw new NotImplementedException();
        }

        public ushort ReadUInt16() {
            throw new NotImplementedException();
        }

        public uint ReadUInt32() {
            throw new NotImplementedException();
        }

        public ulong ReadUInt64() {
            throw new NotImplementedException();
        }

        public byte ReadUInt8() {
            throw new NotImplementedException();
        }

        public void Dispose() { }
    }
}
