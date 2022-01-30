using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Scorpio;
using System.Text;
namespace Scorpio.Conversion.Engine {
    public interface IValue {
        string Value { get; }
    }
    public class ValueString : IValue {
        public string value;
        public ValueString(string value) {
            this.value = value;
        }
        public override string ToString() {
            return "\"" + value.Replace("[", "\\[").Replace(";", "\\;") + "\"";
        }
        public string Value { get { return value; } }
    }
    public class ValueList : IValue {
        public List<IValue> values = new List<IValue>();
        public string Value { get { return ToString(); } }
        public int Count { get { return values.Count; } }
        public IValue this[int i] {
            get { return values[i]; }
        }
        public override string ToString() {
            return "[" + string.Join<IValue>(";", values) + "]";
        }
    }
    public class ValueParser {
        private string buffer;
        private int index;
        private int length;
        public ValueParser(string buffer) {
            this.buffer = buffer;
            this.length = buffer.Length;
            this.index = 0;
        }
        char ReadChar() {
            if (index == length) {
                return (char)0;
            } else {
                return buffer[index++];
            }
        }
        void UndoChar() {
            --index;
        }
        public IValue GetObject() {
            var start = ReadChar();
            if (start == '[') {
                var ret = new ValueList();
                while (true) {
                    var val = GetObject();
                    if (val == null) {
                        break;
                    }
                    ret.values.Add(val);
                }
                return ret;
            } else if (start == (char)0 || start == ']') {
                return null;
            } else if (start == ';' || start == ',' || start == '|' || start == ' ' || start == '\n' || start == '\r') {
                return GetObject();
            } else {
                var builder = new StringBuilder().Append(start);
                while (true) {
                    var ch = ReadChar();
                    if (ch == '\\') {
                        var c = ReadChar();
                        if (c == '[' || c == ';' || ch == ',') {
                            builder.Append(c);
                        } else {
                            builder.Append(ch).Append(c);
                        }
                    } else if (ch == ']') {
                        UndoChar();
                        break;
                    } else if (ch == ';' || ch == ',' || ch == '|') {
                        break;
                    } else {
                        builder.Append(ch);
                    }
                }
                return new ValueString(builder.ToString());
            }
        }

        //public IValue GetObject_Backup() {
        //    var buf = buffer.AsSpan();

        //    var i = 0;

        //    switch (buf[0]) {
        //        case '#':
        //            return ReadListObj(buf.Slice(1), ref i);
        //        default:
        //            return ReadStringObj(buf.Slice(0), ref i);
        //    }
        //}

        //private static IValue ReadListObj(ReadOnlySpan<char> buf, ref int i) {
        //    if (i < 0) throw new ArgumentOutOfRangeException(nameof(i));

        //    var start = i;
        //    var result = new ValueList();

        //    for (; i < buf.Length;) {
        //        switch (buf[i]) {
        //            case '#': {
        //                var subStart = 0;
        //                result.values.Add(ReadStringObj(buf.Slice(start, i - start), ref subStart));
        //                i++;
        //                return result;
        //            }
        //            case '|': {
        //                var subStart = 0;
        //                result.values.Add(ReadStringObj(buf.Slice(start, i - start), ref subStart));
        //                i++;
        //                start = i;
        //            }
        //            break;
        //            default:
        //                i++;
        //                break;
        //        }
        //    }

        //    throw new InvalidDataException("Can not find # for end");
        //}

        //private static IValue ReadStringObj(ReadOnlySpan<char> buf, ref int i) {
        //    if (i < 0) throw new ArgumentOutOfRangeException(nameof(i));

        //    var start = i;

        //    for (; i < buf.Length; i++) {
        //        if (buf[i] != '|' && buf[i] != '#') continue;

        //        var value = new string(buf.Slice(start, i));

        //        if (!value.Contains(';')) return new ValueString(value);

        //        var result = new ValueList();
        //        result.values.AddRange(value.Split(';').Select(_ => new ValueString(_)));
        //        return result;
        //    }

        //    var r = new string(buf);
        //    if (!r.Contains(';')) return new ValueString(r);
        //    {
        //        var result = new ValueList();
        //        result.values.AddRange(r.Split(';').Select(_ => new ValueString(_)));
        //        return result;
        //    }
        //}
    }
}