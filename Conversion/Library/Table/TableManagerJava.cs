using System;
using System.Collections.Generic;
using System.Text;

public partial class TableBuilder
{
    public void CreateManagerJava()
    {
        var code = PROGRAM.Java;
        var programInfo = Util.GetProgramInfo(code);
        var normalClasses = GetNormalClasses(code);
        var spawnsClasses = GetSpawnsClasses(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"package __Package;
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
    private java.util.HashMap<String, __Class> __FilerArray = new java.util.HashMap<String, __Class>();
    public __Class GetSpawns(__Filer key) { return GetSpawns___Filer_impl(key.toString()); }
    public __Class GetSpawns___Filer(String key) { return GetSpawns___Filer_impl(""__Filer_"" + key); }
    private __Class GetSpawns___Filer_impl(String key) {
        if (__FilerArray.containsKey(key))
            return __FilerArray.get(key);
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
        programInfo.CreateFile("TableManager", builder.ToString());
    }
}
