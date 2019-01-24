using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Scorpio;
using Scorpio.Variable;

public class PackageParser {
    private const string ENUM_KEYWORD = "enum_";            //枚举类型关键字
    private const string CONST_KEYWORD = "const_";          //常量类型关键字
    private const string MESSAGE_KEYWORD = "message_";      //消息格式关键字
    private const string TABLE_KEYWORD = "table_";          //Table格式关键字

    private Script mScript = null;
    public Dictionary<string, PackageEnum> Enums { get; set; } = new Dictionary<string, PackageEnum>();
    public Dictionary<string, PackageConst> Consts { get; set; } = new Dictionary<string, PackageConst>();
    public Dictionary<string, PackageClass> Messages { get; set; } = new Dictionary<string, PackageClass>();
    public Dictionary<string, PackageClass> Tables { get; set; } = new Dictionary<string, PackageClass>();
    public Dictionary<string, PackageClass> Classes { get; set; } = new Dictionary<string, PackageClass>();
    void ParseEnum(string name, ScriptTable table) {
        var enums = new PackageEnum();
        var itor = table.GetIterator();
        while (itor.MoveNext()) {
            var fieldName = itor.Current.Key as string;
            var val = itor.Current.Value as ScriptNumber;
            if (string.IsNullOrEmpty(fieldName) || val == null) throw new Exception($"Enum:{name} Field:{fieldName} 参数出错");
            enums.Fields.Add(new FieldEnum() {
                Index = Convert.ToInt32(val.ObjectValue),
                Name = fieldName,
            });
        }
        enums.Fields.Sort((m1, m2) => { return m1.Index.CompareTo(m2.Index); });
        Enums[name.Substring(ENUM_KEYWORD.Length)] = enums;
    }
    void ParseConst(string name, ScriptTable table) {
        var consts = new PackageConst();
        var itor = table.GetIterator();
        while (itor.MoveNext()) {
            var fieldName = itor.Current.Key as string;
            if (string.IsNullOrEmpty(fieldName)) throw new Exception($"Const:{name} Field:{fieldName} 参数出错");
            var pack = new FieldConst() { Name = fieldName };
            ScriptObject value = itor.Current.Value;
            if (value is ScriptNumberDouble) {
                pack.Type = BasicEnum.INT32;
                pack.Value = ((ScriptNumberDouble)value).ToInt32().ToString();
            } else if (value is ScriptNumberLong) {
                pack.Type = BasicEnum.INT64;
                pack.Value = ((ScriptNumberLong)value).ToLong().ToString() + "L";
            } else if (value is ScriptString) {
                pack.Type = BasicEnum.STRING;
                pack.Value = "\"" + value.ToString() + "\"";
            } else {
                throw new Exception("不支持此常量类型 " + value.Type);
            }
            consts.Fields.Add(pack);
        }
        Consts[name.Substring(CONST_KEYWORD.Length)] = consts;
    }
    void ParseClass(string name, ScriptTable table) {
        var classes = new PackageClass();
        var itor = table.GetIterator();
        while (itor.MoveNext()) {
            var fieldName = itor.Current.Key as string;
            var val = itor.Current.Value as ScriptString;
            if (string.IsNullOrEmpty(fieldName) || val == null) throw new Exception($"Class:{name} Field:{fieldName} 参数出错 参数模版 \"索引,类型,是否数组=false,注释\"");
            var infos = val.Value.Split(',');
            if (infos.Length < 2) throw new Exception($"Class:{name} Field:{fieldName} 参数出错 参数模版 \"索引,类型,是否数组=false,注释\"");
            var packageField = new FieldClass() {
                Name = fieldName,
                Index = infos[0].ToInt32(),
                Type = infos[1],
                Array = infos.Length > 2 && infos[2].ToBoolean(),
                Comment = infos.Length > 3 ? infos[3] : "",
            };
            if (!packageField.IsBasic) {
                if (!mScript.HasValue(packageField.Type) &&                             //判断网络协议自定义类
                    !mScript.HasValue(ENUM_KEYWORD + packageField.Type) &&              //判断是否是枚举
                    !mScript.HasValue(MESSAGE_KEYWORD + packageField.Type) &&           //判断网络协议自定义类
                    !mScript.HasValue(TABLE_KEYWORD + packageField.Type)                //判断Table内嵌类
                           ) {
                    throw new Exception($"Class:{name} Field:{fieldName} 未知类型:{packageField.Type}");
                }
            }
            classes.Fields.Add(packageField);
        }
        classes.Fields.Sort((m1, m2) => { return m1.Index.CompareTo(m2.Index); });
        if (name.StartsWith(MESSAGE_KEYWORD)) {         //协议结构
            Messages[name.Substring(MESSAGE_KEYWORD.Length)] = classes;
        } else if (name.StartsWith(TABLE_KEYWORD)) {    //table结构
            Tables[name.Substring(TABLE_KEYWORD.Length)] = classes;
        } else {
            Classes[name] = classes;
        }
    }
    public int GetEnumValue(string name, string value) {
        var enums = Enums[name];
        foreach (var pair in enums.Fields) {
            if (pair.Name == value)
                return pair.Index;
        }
        throw new Exception($"枚举:{name} 找不到枚举值:{value}");
    }
    public PackageClass GetClasses(string name) {
        if (Messages.ContainsKey(name))
            return Messages[name];
        else if (Tables.ContainsKey(name))
            return Tables[name];
        else if (Classes.ContainsKey(name))
            return Classes[name];
        throw new Exception($"找不到自定义类 : {name}");
    }
    public void Parse(string dir, bool clear = false) {
        if (clear) {
            Enums.Clear();
            Consts.Clear();
            Messages.Clear();
            Tables.Clear();
            Classes.Clear();
        }
        mScript = new Script();
        mScript.LoadLibrary();
        var GlobalBasic = new List<ScriptObject>();
        {
            var itor = mScript.GetGlobalTable().GetIterator();
            while (itor.MoveNext())
                GlobalBasic.Add(itor.Current.Value);
        }
        string[] files = Directory.Exists(dir) ? Directory.GetFiles(dir, "*.sco", SearchOption.AllDirectories) : new string[0];
        foreach (var file in files) { mScript.LoadFile(file); }
        {
            var itor = mScript.GetGlobalTable().GetIterator();
            while (itor.MoveNext()) {
                if (GlobalBasic.Contains(itor.Current.Value))
                    continue;
                var name = itor.Current.Key as string;
                var table = itor.Current.Value as ScriptTable;
                if (name == null || table == null) continue;

                if (name.StartsWith(ENUM_KEYWORD)) {                //枚举类型
                    ParseEnum(name, table);
                } else if (name.StartsWith(CONST_KEYWORD)) {        //常量类型
                    ParseConst(name, table);
                } else {
                    ParseClass(name, table);
                }
            }
        }
    }
}
