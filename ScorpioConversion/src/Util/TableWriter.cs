using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
public class TableWriter :IDisposable {
    MemoryStream stream;
    BinaryWriter writer;
    public TableWriter() {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);
    }
    public void Seek(int offset) {
        writer.Seek(offset, SeekOrigin.Begin);
    }
    public void WriteEnum(List<EnumField> fields) {
        WriteInt32(fields.Count);
        foreach (var field in fields) {
            WriteString(field.Name);
            WriteInt32(field.Index);
        }
    }
    public void WriteClass(List<ClassField> fields) {
        WriteInt32(fields.Count);               //字段数量
        foreach (var field in fields) {
            if (field.IsBasic) {
                WriteInt8(0);
                WriteInt8((sbyte)field.BasicType.Index);
            } else {
                WriteInt8(field.IsEnum ? (sbyte)1 : (sbyte)2);
                WriteString(field.Type);
            }
            WriteBool(field.IsArray);
            WriteString((field.IsL10N ? "$" : "") + field.Name);
        }
    }
    public void WriteBool(bool value) {
        writer.Write(value ? (sbyte)1 : (sbyte)0);
    }
    public void WriteInt8(sbyte value) {
        writer.Write(value);
    }
    public void WriteInt32(int value) {
        writer.Write(value);
    }
    public void WriteBool(string value) {
        writer.Write(value.ToBoolean() ? (sbyte)1 : (sbyte)0);
    }
    public void WriteInt8(string value) {
        writer.Write(value.ToInt8());
    }
    public void WriteUInt8(string value) {
        writer.Write(value.ToUInt8());
    }
    public void WriteInt16(string value) {
        writer.Write(value.ToInt16());
    }
    public void WriteUInt16(string value) {
        writer.Write(value.ToUInt16());
    }
    public void WriteInt32(string value) {
        writer.Write(value.ToInt32());
    }
    public void WriteUInt32(string value) {
        writer.Write(value.ToUInt32());
    }
    public void WriteInt64(string value) {
        writer.Write(value.ToInt64());
    }
    public void WriteUInt64(string value) {
        writer.Write(value.ToUInt64());
    }
    public void WriteFloat(string value) {
        writer.Write(value.ToFloat());
    }
    public void WriteDouble(string value) {
        writer.Write(value.ToDouble());
    }
    public void WriteString(string value) {
        if (value.IsEmptyString()) {
            writer.Write((ushort)0);
        } else {
            var bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(Convert.ToUInt16(bytes.Length));
            writer.Write(bytes);
        }
    }
    public void WriteDateTime(string value) {
        double span;
        if (double.TryParse(value, out span)) {
            writer.Write(BasicUtil.GetTimeSpan(DateTime.FromOADate(span)));
            return;
        }
        DateTime datetime;
        if (DateTime.TryParse(value, out datetime)) {
            writer.Write(BasicUtil.GetTimeSpan(datetime));   
            return;
        }
        throw new Exception("不能识别日志字符串 : " + value);
    }
    public byte[] ToArray() {
        stream.Position = 0;
        return stream.ToArray();
    }
    public void Dispose() {
        stream.Close();
        writer.Close();
    }
}

