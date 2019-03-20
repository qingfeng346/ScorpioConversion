using System;
using System.Collections.Generic;
using Scorpio;
using Scorpio.Compiler;
using System.Text;
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
        return "[" + string.Join<IValue>(';', values) + "]";
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
        } else if (start == ';' || start == ',' || start == ' ' || start == '\n' || start == '\r') {
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
                } else if (ch == ';' || ch == ',') {
                    break;
                } else {
                    builder.Append(ch);
                }
            }
            return new ValueString(builder.ToString());
        }
    }
}

