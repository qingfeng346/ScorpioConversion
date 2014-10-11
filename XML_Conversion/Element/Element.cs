using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.IO;


public abstract class Element
{
    public abstract ElementType Type { get; }
    #region 获得代码中的数据类型
    /// <summary> 获得代码中的数据类型 </summary>
    public string GetVariable(PROGRAM program, bool bArray = false)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return bArray ? string.Format("List<{0}>", GetVariable_impl(program)) : GetVariable_impl(program);
            case PROGRAM.JAVA:
                return bArray ? string.Format("ArrayList<{0}>", GetVariable_impl(program)) : GetVariable_impl(program);
            case PROGRAM.PHP:
                return GetVariable_impl(program);
        }
        return "";
    }
    /// 获得代码中的数据类型
    protected virtual string GetVariable_impl(PROGRAM program)
    {
        if (program == PROGRAM.PHP)
            return "$";
        return "";
    }
    #endregion
    #region 获得字段的名字
    public string GetFieldName(PROGRAM program, string strName)
    {
        return "m_" + strName;
    }
    #endregion
    #region 写入数据到Data文件
    public void WriteValueByType(string strValue, BinaryWriter writer, bool bArray)
    {
        if (bArray) {
            if (!Util.IsEmptyString(strValue)) {
                string[] str = strValue.Split(new char[]{','});
                writer.Write((Int32)str.Length);
                for (int i = 0; i < str.Length;++i )
                    WriteValueByType_impl(str[i], writer);
            } else {
                writer.Write((Int32)0);
            }
        } else {
            WriteValueByType_impl(strValue, writer);
        }
    }
    public virtual void WriteValueByType_impl(string strValue, BinaryWriter writer) { }
    #endregion
    #region 获得读取数据的代码
    public string GetReadMemory(PROGRAM program, string strName, string fieldSimpleName, bool bArray)
    {
        string str = "";
        if (bArray) {
            switch (program)
            {
                case PROGRAM.CS:
                    str += @"count = __IntElement.Read();
        for (i = 0;i < count; ++i) {
            __Name.Add(__ReadMemory);
        }";
                    break;
                case PROGRAM.JAVA:
                    str += @"count = __IntElement.Read();
        for (i = 0;i < count; ++i)  {
            __Name.add(__ReadMemory);
        }";
                    break;
                case PROGRAM.PHP:
                    str += @"$count = __IntElement.Read();
        for ($i = 0;$i < $count; ++$i) {
            array_push(__Name,__ReadMemory);
        }";
                    break;
            }
        } else {
            switch (program)
            {
                case PROGRAM.CS:
                case PROGRAM.JAVA:
                case PROGRAM.PHP:
                    str += @"__Name = __ReadMemory;";
                    break;
            }
        }
        str = str.Replace("__Name", strName);
        str = str.Replace("__ReadMemory", GetReadMemory_impl(program, fieldSimpleName));
        return str;
    }
    public virtual string GetReadMemory_impl(PROGRAM program, string strName = "") 
    {
        if (program == PROGRAM.CS)
            return string.Format("reader.Read{0}()", GetVariable_impl(program));
        else if (program == PROGRAM.JAVA)
            return string.Format("reader.Read{0}()", GetVariable_impl(program));
        else if (program == PROGRAM.PHP)
            return string.Format("reader.Read{0}()", GetVariable_impl(program));
        return ""; 
    }
    #endregion
    #region 从Data读取数据 返回Excel原始数据
    public string ReadValueByType(BinaryReader reader, bool bArray)
    {
        string str = "";
        if (bArray) {
            Int32 nCount = reader.ReadInt32();
            if (nCount > 0) {
                bool bFirst = true;
                for (int i = 0; i < nCount; ++i) {
                    if (bFirst == false)
                        str += ",";
                    str += ReadValueByType_impl(reader);
                    bFirst = false;
                }
            } else {
                str = "####";
            }
        } else {
            str = ReadValueByType_impl(reader);
        }
        return str;
    }
    protected virtual string ReadValueByType_impl(BinaryReader reader) { return ""; }
    //自定义类型使用
    public string ReadValueByType(BinaryReader reader, List<int> typeList, bool bArray)
    {
        string str = "";
        if (bArray) {
            Int32 nCount = reader.ReadInt32();
            if (nCount > 0) {
                bool bFirst = true;
                for (int i = 0; i < nCount; ++i) {
                    if (bFirst == false)
                        str += ",";
                    str += ReadValueByType_impl(reader,typeList);
                    bFirst = false;
                }
            } else {
                str = "####";
            }
        } else {
            str = ReadValueByType_impl(reader,typeList);
        }
        return str;
    }
    protected string ReadValueByType_impl(BinaryReader reader, List<int> typeList)
    {
        string str = "";
        bool bFirst = true;
        for (int i = 0; i < typeList.Count; ++i)
        {
            if (bFirst == false)
                str += ":";
            int index = typeList[i];
            Element element = Util.GetElement((ElementType)index);
            str += element.ReadValueByType_impl(reader);
            bFirst = false;
        }
        return str;
    }
    #endregion
}
class BoolElement : Element
{
    public override ElementType Type { get { return ElementType.BOOL; } }
    protected override string GetVariable_impl(PROGRAM program)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "bool";
            case PROGRAM.JAVA:
                return "Boolean";
        }
        return base.GetVariable_impl(program);
    }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        bool value = Util.IsEmptyString(strValue) ? Util.INVALID_BOOL : Util.ToBoolean(strValue);
        writer.Write(value ? (sbyte)1 : (sbyte)0);
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "(reader.ReadSByte().Equals((byte)1) ? true : false)";
            case PROGRAM.JAVA:
                return "(reader.get() == 1 ? true : false)";
            case PROGRAM.PHP:
                return "($reader->readChar() == 1 ? true : false)";
        }
        return base.GetReadMemory_impl(program, strName);
    }
    protected override string ReadValueByType_impl(BinaryReader reader)
    {
        return reader.ReadByte().ToString();
    }
}
class Int8Element : Element
{
    public override ElementType Type { get { return ElementType.INT8; } }
    protected override string GetVariable_impl(PROGRAM program) 
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "sbyte";
            case PROGRAM.JAVA:
                return "byte";
        }
        return base.GetVariable_impl(program);
    }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        writer.Write(Util.IsEmptyString(strValue) ? Util.INVALID_INT8 : Convert.ToSByte(strValue));
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "reader.ReadSByte()";
            case PROGRAM.JAVA:
                return "reader.get()";
            case PROGRAM.PHP:
                return "$reader->readChar()";
        }
        return base.GetReadMemory_impl(program, strName);
    }
    protected override string ReadValueByType_impl(BinaryReader reader)
    {
        return reader.ReadSByte().ToString();
    }
}
class Int16Element : Element
{
    public override ElementType Type { get { return ElementType.INT16; } }
    protected override string GetVariable_impl(PROGRAM program)
    {
        switch (program)
        {
            case PROGRAM.CS:
            case PROGRAM.JAVA:
                return "short";
        }
        return base.GetVariable_impl(program);
    }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        writer.Write(Util.IsEmptyString(strValue) ? Util.INVALID_INT16 : Convert.ToInt16(strValue));
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "reader.ReadInt16()";
            case PROGRAM.JAVA:
                return "reader.getShort()";
            case PROGRAM.PHP:
                return "$reader->readShort()";
        }
        return base.GetReadMemory_impl(program, strName);
    }
    protected override string ReadValueByType_impl(BinaryReader reader)
    {
        return reader.ReadInt16().ToString();
    }
}
class Int32Element : Element
{
    public override ElementType Type { get { return ElementType.INT32; } }
    protected override string GetVariable_impl(PROGRAM program)
    {
        switch (program)
        {
            case PROGRAM.CS:
            case PROGRAM.JAVA:
                return "int";
        }
        return base.GetVariable_impl(program);
    }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        if (Util.IsEmptyString(strValue))
        {
            writer.Write(Util.INVALID_INT32);
        }
        else
        {
            double dValue;
            int value;
            if (int.TryParse(strValue, out value))
            {
                writer.Write(value);
            }
            else if (double.TryParse(strValue, out dValue))
            {
                writer.Write(Convert.ToInt32(dValue));
            }
            else
            {
                throw new Exception("Int32Element 无法转换类型 " + strValue);
            }
        }
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "reader.ReadInt32()";
            case PROGRAM.JAVA:
                return "reader.getInt()";
            case PROGRAM.PHP:
                return "$reader->readInt()";
        }
        return base.GetReadMemory_impl(program, strName);
    }
    protected override string ReadValueByType_impl(BinaryReader reader)
    {
        return reader.ReadInt32().ToString();
    }
}
class Int64Element : Element
{
    public override ElementType Type { get { return ElementType.INT64; } }
    protected override string GetVariable_impl(PROGRAM program)
    {
        switch (program)
        {
            case PROGRAM.CS:
            case PROGRAM.JAVA:
                return "long";
        }
        return base.GetVariable_impl(program);
    }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        writer.Write(Util.IsEmptyString(strValue) ? Util.INVALID_INT64 : Convert.ToInt64(strValue));
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "reader.ReadInt64()";
            case PROGRAM.JAVA:
                return "reader.getLong()";
            case PROGRAM.PHP:
                return "$reader->readLong()";
        }
        return base.GetReadMemory_impl(program, strName);
    }
    protected override string ReadValueByType_impl(BinaryReader reader)
    {
        return reader.ReadInt64().ToString();
    }
}
class FloatElement : Element
{
    public override ElementType Type { get { return ElementType.FLOAT; } }
    protected override string GetVariable_impl(PROGRAM program)
    {
        switch (program)
        {
            case PROGRAM.CS:
            case PROGRAM.JAVA:
                return "float";
        }
        return base.GetVariable_impl(program);
    }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        writer.Write(Util.IsEmptyString(strValue) ? Util.INVALID_FLOAT : Convert.ToSingle(strValue));
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "reader.ReadSingle()";
            case PROGRAM.JAVA:
                return "reader.getFloat()";
            case PROGRAM.PHP:
                return "$reader->readFloat()";
        }
        return base.GetReadMemory_impl(program, strName);
    }
    protected override string ReadValueByType_impl(BinaryReader reader)
    {
        return reader.ReadSingle().ToString();
    }
}
class DoubleElement : Element
{
    public override ElementType Type { get { return ElementType.DOUBLE; } }
    protected override string GetVariable_impl(PROGRAM program)
    {
        switch (program)
        {
            case PROGRAM.CS:
            case PROGRAM.JAVA:
                return "double";
        }
        return base.GetVariable_impl(program);
    }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        writer.Write(Util.IsEmptyString(strValue) ? Util.INVALID_DOUBLE : Convert.ToDouble(strValue));
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "reader.ReadDouble()";
            case PROGRAM.JAVA:
                return "reader.getDouble()";
            case PROGRAM.PHP:
                return "$reader->readDouble()";
        }
        return base.GetReadMemory_impl(program, strName);
    }
    protected override string ReadValueByType_impl(BinaryReader reader)
    {
        return reader.ReadDouble().ToString();
    }
}
class StringElement : Element
{
    public override ElementType Type { get { return ElementType.STRING; } }
    protected override string GetVariable_impl(PROGRAM program)
    {
        switch (program)
        {
            case PROGRAM.CS:
            case PROGRAM.JAVA:
                return "String";
        }
        return base.GetVariable_impl(program);
    }
    public override void WriteValueByType_impl(string strValue, BinaryWriter writer)
    {
        Util.WriteString(writer, strValue);
    }
    public override string GetReadMemory_impl(PROGRAM program, string strName)
    {
        switch (program)
        {
            case PROGRAM.CS:
                return "TableUtil.ReadString(reader)";
            case PROGRAM.JAVA:
                return "TableUtil.ReadString(reader)";
            case PROGRAM.PHP:
                return "$reader->readString()";
        }
        return base.GetReadMemory_impl(program, strName);
    }
    protected override string ReadValueByType_impl(BinaryReader reader)
    {
        return Util.ReadString(reader);
    }
}



