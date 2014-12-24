using System;
using System.Collections.Generic;
using System.Text;

public abstract class IGenerate
{
    protected string m_Package;
    protected string m_ClassName;
    protected List<PackageField> m_Fields;
    protected PROGRAM m_Code;
    public IGenerate(string className, string package, List<PackageField> fields, PROGRAM code)
    {
        m_ClassName = className;
        m_Package = package;
        m_Fields = fields;
        m_Code = code;
    }
    public string GetCodeType(string type)
    {
        var b = BasicUtil.GetType(type);
        return b != null ? b.GetCode(m_Code) : type;
    }
    public abstract string Generate();
}

