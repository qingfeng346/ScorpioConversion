using System;
using System.Collections.Generic;
using System.Text;

public partial class TableBuilder {
    public void CreateManagerJava() {
        var code = PROGRAM.Java;
        var programInfo = Util.GetProgramInfo(code);
        var normalClasses = GetNormalClasses(code);
        var spawnsClasses = GetSpawnsClasses(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"package __Package;
public class TableManager {
    public void Reset() {");
        foreach (var clazz in normalClasses) {
            builder.Append(@"
        m__Filer = null;".Replace("__Filer", clazz.Filer));
        }
        foreach (var clazz in spawnsClasses) {
            builder.Append(@"
        __FilerArray.clear();".Replace("__Filer", clazz.Filer));
        }
        builder.Append(@"
    }");
        foreach (var clazz in normalClasses) {
            string str = @"
    private __Class m__Filer = null;
    public __Class Get__Filer() { if (m__Filer == null) m__Filer = new __Class().Initialize(this, ""__Filer""); return m__Filer; }";
            str = str.Replace("__Class", clazz.Class);
            str = str.Replace("__Filer", clazz.Filer);
            builder.Append(str);
        }
        bool hadLanguage = false;
        foreach (var clazz in spawnsClasses) {
            if (clazz.Filer == "Language") {
                hadLanguage = true;
            }
            string classCode = @"
    public enum __Filer {";
            foreach (string value in clazz.Files) {
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
        __Class spawns = new __Class().Initialize(this, key);
        __FilerArray.put(key, spawns);
        return spawns;
    }";
            classCode = classCode.Replace("__Filer", clazz.Filer);
            classCode = classCode.Replace("__Class", clazz.Class);
            builder.Append(classCode);
        }
        if (hadLanguage == true) {
            builder.Append(GenerateGetLanguageJava());
        }
        builder.Append(@"
}");
        builder = builder.Replace("__Package", mPackage);
        programInfo.CreateFile("TableManager", builder.ToString());
    }
    string GenerateGetLanguageJava() {
        string str = @"
    private String m_Language;
    private java.util.HashMap<String, String> m_Languages = new java.util.HashMap<String, String>();
    public void setLanguage(String language) {
    	m_Language = language;
        m_Languages.clear();
        java.util.HashMap<Integer, DataLanguage> lans = GetSpawns_Language(language).Datas();
        for (DataLanguage data : lans.values()) {
        	m_Languages.put(data.getKey(), data.getText());
        }
    }
    public String getLanguage() {
    	return m_Language;
    }
    public String getLanguageText(String key) {
    	if (m_Languages.containsKey(key)) {
    		return m_Languages.get(key);
    	}
    	return """";
    }";
        return str;
    }
}
