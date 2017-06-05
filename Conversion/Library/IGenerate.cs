using System;
using System.Collections.Generic;
using System.Text;

public abstract class IGenerate {
    protected string m_Package;
    protected string m_ClassName;
    protected List<PackageField> m_Fields;
    protected List<PackageEnum> m_Enums;
    protected List<PackageConst> m_Consts;
    protected DatabaseTable m_Database;
    protected PROGRAM m_Code;
    protected object m_Parameter;
    public IGenerate(PROGRAM code) {
        m_Code = code;
    }
    public string GetCodeType(string type) {
        var b = BasicUtil.GetType(type);
        return b != null ? b.GetCode(m_Code) : type;
    }
    public string GetCodeType(BasicEnum type) {
        var b = BasicUtil.GetType(type);
        return b != null ? b.GetCode(m_Code) : type.ToString();
    }
    public string Generate(string package) {
        m_Package = package;
        return Generate_impl();
    }
    //生成一个自定义类以及表Data类
    public string Generate(string className, string package, List<PackageField> fields, object parameter = null) {
        m_ClassName = className;
        m_Package = package;
        m_Fields = fields;
        m_Parameter = parameter;
        return Generate_impl();
    }
    //生成一个枚举类
    public string Generate(string className, string package, List<PackageEnum> enums) {
        m_ClassName = className;
        m_Package = package;
        m_Enums = enums;
        return Generate_impl();
    }
    //生成一个常量类
    public string Generate(string className, string package, List<PackageConst> consts) {
        m_ClassName = className;
        m_Package = package;
        m_Consts = consts;
        return Generate_impl();
    }
    //生成一个数据库类
    public string Generate(string className, string package, DatabaseTable database) {
        m_ClassName = className;
        m_Package = package;
        m_Database = database;
        return Generate_impl();
    }
    protected abstract string Generate_impl();






    protected string GenerateCSharpToString() {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public override string ToString() {
        return ""{ """);
        var count = m_Fields.Count;
        if (count > 0) {
            for (int i = 0; i < count; ++i) {
                var field = m_Fields[i];
                string str = "";
                if (field.Array) {
                    str += string.Format(@" + 
                ""{0} : "" + {1}", field.Name, "ScorpioUtil.ToString(_" + field.Name + ")");
                } else {
                    str += string.Format(@" + 
                ""{0} : "" + _{0}", field.Name);
                }
                if (i != count - 1) {
                    str += @" + "",""";
                }
                builder.Append(str);
            }
        } else {
            builder.Append(" + \"\"");
        }
        builder.Append(@" + 
                "" }"";
    }");
        return builder.ToString();
    }
    protected string GenerateJavaToString() {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    @Override
    public String toString() {
        return ""{ """);
        var count = m_Fields.Count;
        if (count > 0) {
            for (int i = 0; i < count; ++i) {
                var field = m_Fields[i];
                string str = "";
                if (field.Array) {
                    str += string.Format(@" + 
                ""{0} : "" + {1}", field.Name, "ScorpioUtil.ToString(_" + field.Name + ")");
                } else {
                    str += string.Format(@" + 
                ""{0} : "" + _{0}", field.Name);
                }
                if (i != count - 1) {
                    str += @" + "",""";
                }
                builder.Append(str);
            }
        } else {
            builder.Append(" + \"\"");
        }
        builder.Append(@" + 
                "" }"";
    }");
        return builder.ToString();
    }
}

