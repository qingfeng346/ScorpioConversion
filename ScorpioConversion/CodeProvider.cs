using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
public class CodeProvider
{
    public static CodeProvider instance = null;
    public static CodeProvider GetInstance()
    {
        if (instance == null)
            instance = new CodeProvider();
        return instance;
    }
    Assembly mAssembly = null;
    public void Initialize()
    {
        CSharpCodeProvider Provider = new CSharpCodeProvider();
        CompilerParameters Parameters = new CompilerParameters();
        Parameters.ReferencedAssemblies.Add("System.dll");
        Parameters.ReferencedAssemblies.Add("XML_Conversion.exe");
        Parameters.GenerateExecutable = false;
        Parameters.GenerateInMemory = true;
        Parameters.IncludeDebugInformation = true;
        string[] fileNames = Directory.GetFiles(Util.BaseDirectory, "*.conversion", SearchOption.AllDirectories);
        if (fileNames.Length <= 0) return;
        CompilerResults cr = Provider.CompileAssemblyFromFile(Parameters, fileNames);
        if (cr.Errors.HasErrors) {
            string str = "cs文件编译错误: \n";
            foreach (CompilerError err in cr.Errors) {
                str += (err.ToString() + "\n");
            }
            throw new Exception(str);
        }
        mAssembly = cr.CompiledAssembly;
    }
    public Type[] GetTypes()
    {
        if (mAssembly != null)
            return mAssembly.GetTypes();
        return null;
    }
    public Type GetType(string type)
    {
        if (mAssembly != null)
            return mAssembly.GetType(type);
        return null;
    }
}

