using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.IO;


public abstract class Element
{
    public abstract ElementType Type { get; }
    protected string GetName() { return GetType().Name.Replace("Element", ""); }
    protected MethodInfo GetWriteValue()
    {
        return typeof(TableWriter).GetMethod("Write" + GetName(), new Type[] { typeof(string) });
    }
    protected MethodInfo GetReadValue()
    {
        return typeof(TableReader).GetMethod("Read" + GetName());
    }
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
            case PROGRAM.CPP:
                return bArray ? string.Format("vector<{0}>", GetVariable_impl(program)) : GetVariable_impl(program);
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
    public void WriteValueByType(string strValue, TableWriter writer, bool bArray)
    {
        if (bArray) {
            if (!Util.IsEmptyString(strValue)) {
                string[] str = strValue.Split(new char[]{','});
                writer.WriteInt32((Int32)str.Length);
                for (int i = 0; i < str.Length;++i )
                    WriteValueByType_impl(str[i], writer);
            } else {
                writer.WriteInt32(0);
            }
        } else {
            WriteValueByType_impl(strValue, writer);
        }
    }
    protected virtual void WriteValueByType_impl(string strValue, TableWriter writer)
    {
        GetWriteValue().Invoke(writer, new object[] { strValue });
    }
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
                case PROGRAM.CPP:
                    str += @"count = __IntElement.Read();
        for (i = 0;i < count; ++i)  {
            __Name.push_back(__ReadMemory);
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
                case PROGRAM.CPP:
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
        return string.Format("reader.Read{0}()", GetName());
    }
    #endregion
    #region 从Data读取数据 返回Excel原始数据
    public string ReadValueByType(TableReader reader, bool bArray)
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
                str = Util.EmptyString;
            }
        } else {
            str = ReadValueByType_impl(reader);
        }
        return str;
    }
    protected virtual string ReadValueByType_impl(TableReader reader) { 
        return GetReadValue().Invoke(reader, new object[0]).ToString();
    }
    //自定义类型使用
    public string ReadValueByType(TableReader reader, List<int> typeList, bool bArray)
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
                str = Util.EmptyString;
            }
        } else {
            str = ReadValueByType_impl(reader,typeList);
        }
        return str;
    }
    protected string ReadValueByType_impl(TableReader reader, List<int> typeList)
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
            case PROGRAM.CPP:
                return "bool";
        }
        return base.GetVariable_impl(program);
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
            case PROGRAM.CPP:
                return "char";
        }
        return base.GetVariable_impl(program);
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
            case PROGRAM.CPP:
                return "short";
        }
        return base.GetVariable_impl(program);
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
            case PROGRAM.CPP:
                return "int";
        }
        return base.GetVariable_impl(program);
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
            case PROGRAM.CPP:
                return "long";
        }
        return base.GetVariable_impl(program);
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
            case PROGRAM.CPP:
                return "float";
        }
        return base.GetVariable_impl(program);
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
            case PROGRAM.CPP:
                return "double";
        }
        return base.GetVariable_impl(program);
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
            case PROGRAM.CPP:
                return "string";
        }
        return base.GetVariable_impl(program);
    }
}