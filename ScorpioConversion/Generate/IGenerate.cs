using System;
using System.Collections.Generic;
using System.Text;

public abstract class IGenerate
{
    protected string m_ClassName;
    protected List<PackageField> m_Fields;
    protected PROGRAM m_Code;
    public IGenerate(PROGRAM code)
    {
        m_Code = code;
    }
    public string GetCodeType(string type)
    {
        var b = BasicUtil.GetType(type);
        return b != null ? b.GetCode(m_Code) : type;
    }
    public string Generate(string className, List<PackageField> fields)
    {
        m_ClassName = className;
        m_Fields = fields;
        return Generate_impl();
    }
    protected abstract string Generate_impl();
}

