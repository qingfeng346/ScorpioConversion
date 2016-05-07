using System;
using System.Collections.Generic;
using System.Text;

public partial class TableBuilder
{
    public void CreateManagerCPP()
    {
        var code = PROGRAM.CPP;
        var programInfo = Util.GetProgramInfo(code);
        var normalClasses = GetNormalClasses(code);
        var spawnsClasses = GetSpawnsClasses(code);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(@"#ifndef ____TableManager_H__
#define ____TableManager_H__");
        builder.AppendLine(TemplateCPP.Head);
        foreach (var clazz in normalClasses) {
            builder.AppendLine(string.Format("#include \"{0}.h\"", clazz.Class)) ;
        }
        foreach (var clazz in spawnsClasses) {
            builder.AppendLine(string.Format("#include \"{0}.h\"", clazz.Class));
        }
        string[] packages = mPackage.Split('.');
        foreach (var package in packages) {
            builder.AppendLine("namespace " + package + "{");
        }

        builder.Append(@"
class TableManager {
    public: void Reset() {");
        foreach (var clazz in normalClasses) {
            builder.Append(@"
        delete m__Filer; m__Filer = nullptr;".Replace("__Filer", clazz.Filer));
        }
        foreach (var clazz in spawnsClasses) {
            builder.Append(@"
        __FilerArray.clear();".Replace("__Filer", clazz.Filer));
        }
        builder.Append(@"
    }");
        foreach (var clazz in normalClasses) {
            string str = @"
    private: __Class * m__Filer = nullptr;
    public: __Class * Get__Filer() { if (m__Filer == nullptr) { m__Filer = new __Class(); m__Filer->Initialize(""__Filer""); } return m__Filer; }";
            str = str.Replace("__Class", clazz.Class);
            str = str.Replace("__Filer", clazz.Filer);
            builder.Append(str);
        }
        foreach (var clazz in spawnsClasses) {
            string enumName = clazz.Filer;
            string classCode = @"
    private: std::unordered_map<char*, __Class*> __FilerArray;
    private: __Class * GetSpawns___Filer(char * key) {
        if (__FilerArray.find(key) != __FilerArray.end())
            return __FilerArray[key];
        __Class * data = new __Class();
        data->Initialize(key);
        return __FilerArray[key] = data;
    }";
            classCode = classCode.Replace("__Filer", clazz.Filer);
            classCode = classCode.Replace("__Class", clazz.Class);
            builder.Append(classCode);
        }
        builder.Append(@"
};
");
        foreach (var package in packages) {
            builder.AppendLine("}");
        }
        builder.Append("#endif");
        programInfo.CreateFile("TableManager", builder.ToString());
    }
}
