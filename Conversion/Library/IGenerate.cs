using System;
using System.Collections.Generic;
using System.Text;

public abstract class IGenerate
{
    protected string m_Package;
    protected string m_ClassName;
    protected List<PackageField> m_Fields;
    protected List<PackageEnum> m_Enums;
    protected List<PackageConst> m_Consts;
    protected DatabaseTable m_Database;
    protected PROGRAM m_Code;
    protected object m_Parameter;
    public IGenerate(PROGRAM code)
    {
        m_Code = code;
    }
    public string GetCodeType(string type)
    {
        var b = BasicUtil.GetType(type);
        return b != null ? b.GetCode(m_Code) : type;
    }
    public string GetCodeType(BasicEnum type)
    {
        var b = BasicUtil.GetType(type);
        return b != null ? b.GetCode(m_Code) : type.ToString();
    }
    //生成一个自定义类以及表Data类
    public string Generate(string className, string package, List<PackageField> fields, object parameter = null)
    {
        m_ClassName = className;
        m_Package = package;
        m_Fields = fields;
        m_Parameter = parameter;
        return Generate_impl();
    }
    //生成一个枚举类
    public string Generate(string className, string package, List<PackageEnum> enums)
    {
        m_ClassName = className;
        m_Package = package;
        m_Enums = enums;
        return Generate_impl();
    }
    //生成一个常量类
    public string Generate(string className, string package, List<PackageConst> consts)
    {
        m_ClassName = className;
        m_Package = package;
        m_Consts = consts;
        return Generate_impl();
    }
    //生成一个数据库类
    public string Generate(string className, string package, DatabaseTable database)
    {
        m_ClassName = className;
        m_Package = package;
        m_Database = database;
        return Generate_impl();
    }
    protected abstract string Generate_impl();
}

