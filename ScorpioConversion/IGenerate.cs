using System;
using System.Collections.Generic;
using System.Text;

public abstract class IGenerate
{
    protected string m_Package;
    protected string m_ClassName;
    protected List<PackageField> m_Fields;
    protected List<string> m_EnumArray;
    protected List<PackageEnum> m_Enums;
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
    public string Generate(string className, string package, List<PackageField> fields, List<string> enumsArray,object parameter)
    {
        m_ClassName = className;
        m_Package = package;
        m_Fields = fields;
        m_EnumArray = enumsArray;
        m_Parameter = parameter;
        return Generate_impl();
    }
    public string Generate(string className, string package, List<PackageEnum> enums)
    {
        m_ClassName = className;
        m_Package = package;
        m_Enums = enums;
        return Generate_impl();
    }
    protected abstract string Generate_impl();
}

