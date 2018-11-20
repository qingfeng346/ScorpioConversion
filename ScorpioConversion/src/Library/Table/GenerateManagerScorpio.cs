using System;
using System.Collections.Generic;
using System.Text;

public partial class TableBuilder {
    public void CreateManagerScorpio() {
        var code = PROGRAM.Scorpio;
        var programInfo = Util.GetProgramInfo(code);
        var normalClasses = GetNormalClasses(code);
        var spawnsClasses = GetSpawnsClasses(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//Package = __Package
TableManager = {
    function Reset() {");
        foreach (var clazz in normalClasses) {
            builder.Append(@"
        this.m__Filer = null".Replace("__Filer", clazz.Filer));
        }
        foreach (var clazz in spawnsClasses) {
            foreach (string value in clazz.Files) {
                builder.Append(@"
        Table__Element = null".Replace("__Element", value));
            }
        }
        builder.Append(@"
    }");
        foreach (var clazz in normalClasses) {
            string str = @"
    m__Filer = null,
    function Get__Filer() { if (this.m__Filer == null){ this.m__Filer = __Class.Initialize(this, ""__Filer""); } return this.m__Filer; }";
            str = str.Replace("__Class", clazz.Class);
            str = str.Replace("__Filer", clazz.Filer);
            builder.Append(str);
        }
        bool hadLanguage = false;
        foreach (var clazz in spawnsClasses) {
            if (clazz.Filer == "Language") {
                hadLanguage = true;
            }
            foreach (string value in clazz.Files) {
                string str = @"
    function Get__Element() { if (Table__Element == null){ Table__Element = clone(__Class).Initialize(this, ""__Element""); } return Table__Element; }";
                str = str.Replace("__Element", value);
                str = str.Replace("__Class", clazz.Class);
                builder.Append(str);
            }
        }
        if (hadLanguage == true) {
            builder.Append(GenerateGetValueScorpio());
            builder.Append(GenerateGetLanguageScorpio());
        }
        builder.Append(@"
}");
        builder = builder.Replace("__Package", mPackage);
        programInfo.CreateFile("TableManager", builder.ToString());
    }
    string GenerateGetValueScorpio() {
        string str = @"
    function getValue(attribute, fileName, name, id) {
        if (attribute.Language) {
            return this.getLanguageText(fileName + ""_"" + name + ""_"" + id)
        }
        return """";
    }";
        return str;
    }
    string GenerateGetLanguageScorpio() {
        string str = @"
    m_Language = null,
    m_Languages = {},
    function setLanguage(language) {
    	this.m_Language = language
        this.m_Languages = {}
        var lans = this[""GetLanguage_"" + language]().Datas()
	    foreach (var data in vpairs(lans)) {
            this.m_Languages[data.Key] = data.Text
	    }
    }
    function getLanguage() {
    	return this.m_Language;
    }
    function getLanguageText(key) {
        if (table.containskey(this.m_Languages, key)) {
            return this.m_Languages[key]
        }
    	return """";
    }";
        return str;
    }
}
