using System;
using System.Collections.Generic;
using System.Text;

public partial class TableBuilder
{
    public void CreateManagerCSharp()
    {
        var code = PROGRAM.CSharp;
        var programInfo = Util.GetProgramInfo(code);
        var normalClasses = GetNormalClasses(code);
        var spawnsClasses = GetSpawnsClasses(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"using System;
using System.Collections.Generic;
namespace __Package {
public class TableManager {
    public void Reset() {");
        foreach (var clazz in normalClasses)
        {
            builder.Append(@"
        m__Filer = null;".Replace("__Filer", clazz.Filer));
        }
        foreach (var clazz in spawnsClasses)
        {
            builder.Append(@"
        __FilerArray.Clear();".Replace("__Filer", clazz.Filer));
        }
        builder.Append(@"
    }");
        foreach (var clazz in normalClasses)
        {
            string str = @"
    private __Class m__Filer = null;
    public __Class Get__Filer() { if (m__Filer == null) m__Filer = new __Class().Initialize(""__Filer""); return m__Filer; }";
            str = str.Replace("__Class", clazz.Class);
            str = str.Replace("__Filer", clazz.Filer);
            builder.Append(str);
        }
        foreach (var clazz in spawnsClasses)
        {
            string enumName = clazz.Filer;
            string classCode = @"
    public enum __Filer {";
            foreach (string value in clazz.Files)
            {
                classCode += @"
        __Element,".Replace("__Element", value);
            }
            classCode += @"
    }
    private Dictionary<string, __Class> __FilerArray = new Dictionary<string, __Class>();
    public __Class GetSpawns(__Filer key) { return GetSpawns___Filer_impl(key.ToString()); }
    public __Class GetSpawns___Filer(string key) { return GetSpawns___Filer_impl(""__Filer_"" + key); }
    private __Class GetSpawns___Filer_impl(string key) { return __FilerArray.ContainsKey(key) ? __FilerArray[key] : __FilerArray[key] = new __Class().Initialize(key); }";
            classCode = classCode.Replace("__Filer", clazz.Filer);
            classCode = classCode.Replace("__Class", clazz.Class);
            builder.Append(classCode);
        }
        builder.Append(@"
}
}");
        builder = builder.Replace("__Package", mPackage);
        FileUtil.CreateFile(programInfo.GetFile("TableManager"), builder.ToString(), programInfo.Bom, programInfo.CodeDirectory.Split(';'));
    }
    public void CreateManagerJava()
    {
        var code = PROGRAM.Java;
        var programInfo = Util.GetProgramInfo(code);
        var normalClasses = GetNormalClasses(code);
        var spawnsClasses = GetSpawnsClasses(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"package __Package;
import java.util.HashMap;
public class TableManager {
    public void Reset() {");
        foreach (var clazz in normalClasses)
        {
            builder.Append(@"
        m__Filer = null;".Replace("__Filer", clazz.Filer));
        }
        foreach (var clazz in spawnsClasses)
        {
            builder.Append(@"
        __FilerArray.clear();".Replace("__Filer", clazz.Filer));
        }
        builder.Append(@"
    }");
        foreach (var clazz in normalClasses)
        {
            string str = @"
    private __Class m__Filer = null;
    public __Class Get__Filer() throws Exception { if (m__Filer == null) m__Filer = new __Class().Initialize(""__Filer""); return m__Filer; }";
            str = str.Replace("__Class", clazz.Class);
            str = str.Replace("__Filer", clazz.Filer);
            builder.Append(str);
        }
        foreach (var clazz in spawnsClasses)
        {
            string enumName = clazz.Filer;
            string classCode = @"
    public enum __Filer {";
            foreach (string value in clazz.Files)
            {
                classCode += @"
        __Element,".Replace("__Element", value);
            }
            classCode += @"
    }
    private HashMap<String, __Class> __FilerArray = new HashMap<String, __Class>();
    public __Class GetSpawns(__Filer key) throws Exception { return GetSpawns___Filer_impl(key.toString()); }
    public __Class GetSpawns___Filer(String key) throws Exception { return GetSpawns___Filer_impl(""__Filer_"" + key); }
    private __Class GetSpawns___Filer_impl(String key) throws Exception {
        if (__FilerArray.containsKey(key)) { return __FilerArray.get(key); }
        __Class spawns = new __Class().Initialize(key);
        __FilerArray.put(key, spawns);
        return spawns;
    }";
            classCode = classCode.Replace("__Filer", clazz.Filer);
            classCode = classCode.Replace("__Class", clazz.Class);
            builder.Append(classCode);
        }
        builder.Append(@"
}");
        builder = builder.Replace("__Package", mPackage);
        FileUtil.CreateFile(programInfo.GetFile("TableManager"), builder.ToString(), programInfo.Bom, programInfo.CodeDirectory.Split(';'));
    }
    public void CreateManagerScorpio()
    {
        var code = PROGRAM.Scorpio;
        var programInfo = Util.GetProgramInfo(code);
        var normalClasses = GetNormalClasses(code);
        var spawnsClasses = GetSpawnsClasses(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
TableManager = {
    function Reset() {");
        foreach (var clazz in normalClasses)
        {
            builder.Append(@"
        this.m__Filer = null".Replace("__Filer", clazz.Filer));
        }
        foreach (var clazz in spawnsClasses)
        {
            builder.Append(@"
        this.__FilerArray = {}".Replace("__Filer", clazz.Filer));
        }
        builder.Append(@"
    }");
        foreach (var clazz in normalClasses)
        {
            string str = @"
    m__Filer = null,
    function Get__Filer() { if (m__Filer == null){ m__Filer = __Class.Initialize(""__Filer""); } return m__Filer; }";
            str = str.Replace("__Class", clazz.Class);
            str = str.Replace("__Filer", clazz.Filer);
            builder.Append(str);
        }
        foreach (var clazz in spawnsClasses)
        {
            string enumName = clazz.Filer;
            foreach (string value in clazz.Files)
            {
                string str = @"
    m__Element = null,
    function Get__Element() { if (m__Element == null){ m__Element = clone(__Class).Initialize(""__Element""); } return m__Element; }";
                str = str.Replace("__Element", value);
                str = str.Replace("__Class", clazz.Class);
                builder.Append(str);
            }
        }
        builder.Append(@"
}");
        FileUtil.CreateFile(programInfo.GetFile("TableManager"), builder.ToString(), programInfo.Bom, programInfo.CodeDirectory.Split(';'));
    }
}

