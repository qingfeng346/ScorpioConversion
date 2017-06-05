using System;
using System.Collections.Generic;
using System.Text;

public partial class TableBuilder {
    public void CreateManagerCSharp() {
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
        foreach (var clazz in normalClasses) {
            builder.Append(@"
        m__Filer = null;".Replace("__Filer", clazz.Filer));
        }
        foreach (var clazz in spawnsClasses) {
            builder.Append(@"
        __FilerArray.Clear();".Replace("__Filer", clazz.Filer));
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
    private Dictionary<string, __Class> __FilerArray = new Dictionary<string, __Class>();
    public __Class GetSpawns(__Filer key) { return GetSpawns___Filer_impl(key.ToString()); }
    public __Class GetSpawns___Filer(string key) { return GetSpawns___Filer_impl(""__Filer_"" + key); }
    private __Class GetSpawns___Filer_impl(string key) { 
        if (__FilerArray.ContainsKey(key))
            return __FilerArray[key];
        return __FilerArray[key] = new __Class().Initialize(this, key);
    }";
            classCode = classCode.Replace("__Filer", clazz.Filer);
            classCode = classCode.Replace("__Class", clazz.Class);
            builder.Append(classCode);
        }
        if (hadLanguage == true) {
            builder.Append(GenerateGetLanguageCSharp());
        }
        builder.Append(@"
}
}");
        builder = builder.Replace("__Package", mPackage);
        programInfo.CreateFile("TableManager", builder.ToString());
    }
    string GenerateGetLanguageCSharp() {
        string str = @"
    private String m_Language;
    private Dictionary<String, String> m_Languages = new Dictionary<String, String>();
    public void setLanguage(String language) {
    	m_Language = language;
        m_Languages.Clear();
        Dictionary<int, DataLanguage> lans = GetSpawns_Language(language).Datas();
        foreach (DataLanguage data in lans.Values) {
        	m_Languages[data.getKey()] = data.getText();
        }
    }
    public String getLanguage() {
    	return m_Language;
    }
    public String getLanguageText(String key) {
    	if (m_Languages.ContainsKey(key)) {
    		return m_Languages[key];
    	}
    	return """";
    }";
        return str;
    }
}
