using System;
using System.Collections.Generic;

public static partial class Util
{
    /// <summary>
    /// 获得Data类的变量代码
    /// </summary>
    private static string GetDataFields(PROGRAM program, string strDataName, List<Variable> variables, bool bArray, bool bData, bool bUsable)
    {
        string str = "";
        bool bFirst = true;
        string arrayVariable = "";
        for (int i = 0; i < variables.Count; ++i)
        {
            string temp = "";
            Variable variable = variables[i];
            Element element = Util.GetElement(variable.strFieldType);
            string strVariable = element.GetVariable(program, variable.bArray);
            string strFieldName = variable.strFieldName;
            string strName = element.GetFieldName(program, variable.strFieldName);
            if (program == PROGRAM.CS)
            {
                temp += @"
    private __Variable __Name __NewArray;";
                if (bFirst && bData)
                {
                    temp += @"
    /// <summary> __FieldNote </summary>
    public __Variable ID() { return __Name; }";
                }
                if (!bArray) {
                    temp += @"
    /// <summary> __FieldNote </summary>
    public __Variable __FieldName() { return __Name; }";
                }
            }
            else if (program == PROGRAM.JAVA)
            {
                temp += @"
    private __Variable __Name __NewArray;";
                if (bFirst && bData)
                {
                    temp += @"
    /** __FieldNote */
    public __Variable ID() { return __Name; }";
                }
                if (!bArray)
                {
                    temp += @"
    /** __FieldNote */
    public __Variable __FieldName() { return __Name; }";
                }
            }
            else if (program == PROGRAM.CPP)
            {
                temp += @"
    private: __Variable __Name;";
                if (bFirst && bData)
                {
                    temp += @"
    /** __FieldNote */
    public: __Variable ID() { return __Name; }";
                }
                if (!bArray)
                {
                    temp += @"
    /** __FieldNote */
    public: __Variable __FieldName() { return __Name; }";
                }
            }
            else if (program == PROGRAM.PHP)
            {
                temp += @"
    private $__Name __NewArray;       ////__FieldNote";
                if (bFirst && bData)
                {
                    temp += @"
    public function ID() { return $this->__Name; }";
                }
                if (!bArray)
                {
                    temp += @"
    public function __FieldName() { return $this->__Name; }";
                }
            }
            if (program == PROGRAM.PHP)
                temp = temp.Replace("__NewArray", variable.bArray ? "= array()" : "");
            else
                temp = temp.Replace("__NewArray", variable.bArray ? "= new __Variable()" : "");
            temp = temp.Replace("__Variable", strVariable);
            temp = temp.Replace("__Name", strName);
            temp = temp.Replace("__FieldNameCase", FileUtil.ToOneUpper(strName));
            temp = temp.Replace("__FieldName", strFieldName);
            temp = temp.Replace("__FieldNote", variable.strFieldNote);
            str += temp;
            arrayVariable = strVariable;
            bFirst = false;
        }
        if (bArray && !string.IsNullOrEmpty(arrayVariable))
        {
            Variable variable = variables[1];
            Element element = Util.GetElement(variable.strFieldType);
            string strVariable = element.GetVariable(program, variable.bArray);
            string strName = element.GetFieldName(program, variable.strFieldName);
            if (program == PROGRAM.CS)
            {
                str += @"
    private List<__Variable> m_Array = new List<__Variable>();
    public List<__Variable> Arrays() { return m_Array; }";
            }
            else if (program == PROGRAM.JAVA)
            {
                str += @"
    private ArrayList<__Variable> m_Array = new ArrayList<__Variable>();
    public ArrayList<__Variable> Arrays() { return m_Array; }";
            }
            else if (program == PROGRAM.CPP)
            {
                str += @"
    private: vector<__Variable> m_Array;
    public: vector<__Variable> Arrays() { return m_Array; }";
            }
            else if (program == PROGRAM.PHP)
            {
                str += @"
    private $m_Array = array();
    public function Arrays() { return $this->m_Array; }";
            }
            str = str.Replace("__Variable", arrayVariable);
        }
        return str;
    }
    /// <summary>
    /// 获得Data类的ReadMemory函数
    /// </summary>
    private static string GetDataReadMemory(PROGRAM program, string strDataName, List<Variable> variables, bool bArray, bool bData, bool bUsable)
    {
        string str = "";
        bool hasArray = false;
        for (int i = 0; i < variables.Count; ++i) {
            if (variables[i].bArray == true) { hasArray = true; break; }
        }
        Element intElement = GetElement(ElementType.INT32);
        if (program == PROGRAM.CS)
        {
            str += @"
    public static __DataClass ReadMemory ( TableReader reader, string fileName ) {
        __DataClass Data = new __DataClass();
";
            if (hasArray) {
                str += @"        __IntElement count = 0, i = 0;
";
            }
        }
        else if (program == PROGRAM.JAVA)
        {
            str += @"
    public static __DataClass ReadMemory ( TableReader reader, String fileName ) {
        __DataClass Data = new __DataClass();
";
            if (hasArray) {
                str += @"        __IntElement count = 0, i = 0;
";
            }
        }
        else if (program == PROGRAM.CPP)
        {
            str += @"
    public: static __DataClass ReadMemory (TableReader reader, string fileName) {
        __DataClass Data;
";
            if (hasArray) {
                str += @"        __IntElement count = 0, i = 0;
";
            }
        }
        else if (program == PROGRAM.PHP)
        {
            str += @"
    public static function ReadMemory ( TableReader $reader, $fileName ) {
        $Data = new __DataClass();
";
        }
        str = str.Replace("__IntElement", intElement.GetVariable(program));
        str = str.Replace("__DataClass", strDataName);
        bool bFirst = true;
        for (int i = 0; i < variables.Count; ++i)
        {
            Variable variable = variables[i];
            Element element = Util.GetElement(variable.strFieldType);
            string fieldName = element.GetFieldName(program, variable.strFieldName);
            if (program == PROGRAM.PHP)
                fieldName = "$Data->" + fieldName;
            else
                fieldName = "Data." + fieldName;
            str += string.Format("{0}{1}{2}", Util.GetTab(2), element.GetReadMemory(program, fieldName, variable.strFieldName, variable.bArray), Util.ReturnString);
            if (bArray && bFirst == false) {
                if (program == PROGRAM.CS) {
                    str += string.Format("{0}Data.m_Array.Add({1});{2}", Util.GetTab(2), fieldName, Util.ReturnString);
                } else if (program == PROGRAM.JAVA) {
                    str += string.Format("{0}Data.m_Array.add({1});{2}", Util.GetTab(2), fieldName, Util.ReturnString);
                } else if (program == PROGRAM.PHP) {
                    str += string.Format("{0}array_push($Data->m_Array,{1});{2}", Util.GetTab(2), fieldName, Util.ReturnString);
                }
            }
            bFirst = false;
        }
        if (program == PROGRAM.PHP) {
            str += @"        return $Data;";
        } else if (program == PROGRAM.CPP) {
            str += @"        return Data;";
        } else {
            str += @"        Data.__DataString = Data.ToString_impl();
        return Data;";
        }
        str += @"
    }";
        return str;
    }
    /// <summary>
    /// 获得Data类的IsInvalid函数
    /// </summary>
    private static string GetDataIsInvalid(PROGRAM program, string strDataName, List<Variable> variables, bool bArray, bool bData, bool bUsable)
    {
        string str = "";
        if (program == PROGRAM.CS)
        {
            str += @"
    public override bool IsInvalid () {";
        }
        else if (program == PROGRAM.JAVA)
        {
            str += @"
    @Override
    public boolean IsInvalid () {";
        }
        else if (program == PROGRAM.CPP)
        {
            str += @"
    public: bool IsInvalid() {";
        }
        else if (program == PROGRAM.PHP)
        {
            str += @"
    public function IsInvalid () {";
        }
        for (int i = 0; i < variables.Count; ++i)
        {
            Variable variable = variables[i];
            Element element = Util.GetElement(variable.strFieldType);
            string fieldName = element.GetFieldName(program, variable.strFieldName);
            if (program == PROGRAM.PHP)
                fieldName = "$this->" + fieldName;
            else if (program == PROGRAM.CPP)
                fieldName = "this->" + fieldName;
            else
                fieldName = "this." + fieldName;
            if (variable.bArray == true)
            {
                if (program == PROGRAM.CS)
                {
                    str += @"
        if (__FieldName.Count > 0) return false;";
                }
                else if (program == PROGRAM.JAVA)
                {
                    str += @"
        if (__FieldName.size() > 0) return false;";
                }
                else if (program == PROGRAM.CPP)
                {
                    str += @"
        if (__FieldName.size() > 0) return false;";
                }
                else if (program == PROGRAM.PHP)
                {
                    str += @"
        if (count(__FieldName) > 0) return false;";
                }
            }
            else
            {
                str += @"
        if (!TableUtil.IsInvalid(__FieldName)) return false;";
            }
            str = str.Replace("__FieldName", fieldName);
        }
        str += @"
        return true;
    }";
        return str;
    }
    /// <summary>
    /// 获得Data类的GetDataByString函数
    /// </summary>
    private static string GetDataGetDataByString(PROGRAM program, List<Variable> variables, bool bArray)
    {
        if (program == PROGRAM.CPP) return "";
        string str = "";
        if (program == PROGRAM.CS)
        {
            str += @"
    public override object GetDataByString ( String str ) {";
            if (bArray)
            {
                str += @"
            return null;
        }";
                return str;
            }
            foreach (Variable pair in variables)
            {
                Element element = Util.GetElement(pair.strFieldType);
                str += @"
        if (str == """ + pair.strFieldName + @""") return " + pair.strFieldName + "();";
            }
            str += @"
        return null;
    }";
        }
        else if (program == PROGRAM.JAVA)
        {
            str += @"
    @Override
    public Object GetDataByString ( String str ) throws Exception {";
            if (bArray)
            {
                str += @"
            return null;
        }";
                return str;
            }
            foreach (Variable pair in variables)
            {
                Element element = Util.GetElement(pair.strFieldType);
                str += @"
        if (str.equals(""" + pair.strFieldName + @""")) return " + pair.strFieldName + "();";
            }
            str += @"
            return null;
    }";
        }
        return str;
    }
    /// <summary>
    /// 获得Data类的GetDataByString函数
    /// </summary>
    private static string GetDataToString(PROGRAM program, List<Variable> variables, bool bArray)
    {
        if (program == PROGRAM.CPP) return "";
        string str = "";
        if (program == PROGRAM.CS)
        {
            str += @"
    public override string ToString ( ) {
        return __DataString;
    }
    private string ToString_impl ( ) {
        String str = """";";
            if (!bArray)
            {
                foreach (Variable pair in variables)
                {
                    string temp = @"
        str += (""__FieldName ="" + __FieldName() + '\n');";
                    str += temp.Replace("__FieldName", pair.strFieldName);
                }
            }
        }
        else if (program == PROGRAM.JAVA)
        {
            str += @"
    @Override
    public String toString ( ) {
        return __DataString;
    }
    private string ToString_impl ( ) {
        String str = """";";
            if (!bArray)
            {
                foreach (Variable pair in variables)
                {
                    string temp = @"
        str += (""__FieldName ="" + __FieldName() + '\n');";
                    str += temp.Replace("__FieldName", pair.strFieldName);
                }
            }
        }
        str += @"
        return str;
    }";
        return str;
    }
    /// <summary>
    /// 获得Data类的代码
    /// </summary>
    public static string GetDataClass(PROGRAM program, string strDataName, List<Variable> variables, bool bArray, bool bData, bool bUsable)
    {
        string str = "";
        switch (program)
        {
            case PROGRAM.CS:
                str += @"
public class __DataClass : MT_DataBase {
    public static __StringElement MD5 = ""__DataMD5Code"";
    private string __DataString = """";";
                break;
            case PROGRAM.JAVA:
                str += @"
public class __DataClass extends MT_DataBase {
    public static __StringElement MD5 = ""__DataMD5Code"";
    private string __DataString = """";";
                break;
            case PROGRAM.PHP:
                str += @"
class __DataClass {
    const MD5 = '__DataMD5Code';";
                break;
            case PROGRAM.CPP:
                str += @"
#define __DataClass_MD5 = ""__DataMD5Code"";
class __DataClass : public MT_DataBase {";
                break;
        }
        str += GetDataFields(program, strDataName, variables, bArray, bData, bUsable);
        str += GetDataIsInvalid(program, strDataName, variables, bArray, bData, bUsable);
        str += GetDataGetDataByString(program, variables, bArray);
        if (program != PROGRAM.PHP)
            str += GetDataToString(program, variables, bArray);
        str += GetDataReadMemory(program, strDataName, variables, bArray, bData, bUsable);
        Element intElement = Util.INT_ELEMENT;
        Element strElement = Util.STRING_ELEMENT;
        Element boolElement = Util.BOOL_ELEMENT;
        str = str.Replace("__IntElement.Read()", intElement.GetReadMemory_impl(program));
        str = str.Replace("__StringElement.Read()", strElement.GetReadMemory_impl(program));
        str = str.Replace("__BoolElement.Read()", boolElement.GetReadMemory_impl(program));
        str = str.Replace("__IntElement", intElement.GetVariable(program));
        str = str.Replace("__StringElement", strElement.GetVariable(program));
        str = str.Replace("__BoolElement", boolElement.GetVariable(program));
        str = str.Replace("__DataClass", strDataName);
        str = str.Replace("__DataMD5Code", GetDataMD5Code(variables));
        if (program == PROGRAM.CPP) {
            str += @"
};";
        } else {
            str += @"
}";
        }
        return str;
    }
}