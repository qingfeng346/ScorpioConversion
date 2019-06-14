using System;
using System.Collections.Generic;
using System.IO;
using Scorpio;
using Scorpio.Userdata;

public interface IPackage {

}
public class PackageParser {
    private const string ENUM_KEYWORD = "enum_";            //枚举类型关键字
    private const string CONST_KEYWORD = "const_";          //常量类型关键字
    private const string MESSAGE_KEYWORD = "message_";      //消息格式关键字
    private const string TABLE_KEYWORD = "table_";          //Table格式关键字

    public Dictionary<string, PackageEnum> Enums { get; set; } = new Dictionary<string, PackageEnum>();
    public Dictionary<string, PackageConst> Consts { get; set; } = new Dictionary<string, PackageConst>();
    public Dictionary<string, PackageClass> Messages { get; set; } = new Dictionary<string, PackageClass>();
    public Dictionary<string, PackageClass> Tables { get; set; } = new Dictionary<string, PackageClass>();
    public Dictionary<string, PackageClass> Classes { get; set; } = new Dictionary<string, PackageClass>();
    public Script Script { get; private set; } = new Script();
    void ParseEnum(string name, ScriptMap table) {
        var enums = new PackageEnum();
        foreach (var pair in table) {
            var fieldName = pair.Key as string;
            if (string.IsNullOrEmpty(fieldName)) throw new Exception($"Enum:{name} Field:{fieldName} 参数出错");
            enums.Fields.Add(new FieldEnum() {
                Name = fieldName,
                Index = pair.Value.ToInt32(),
            });
        }
        enums.Fields.Sort((m1, m2) => { return m1.Index.CompareTo(m2.Index); });
        name = name.Substring(ENUM_KEYWORD.Length);
        enums.Name = name;
        Enums[name] = enums;
    }
    void ParseConst(string name, ScriptMap table) {
        var consts = new PackageConst();
        foreach (var pair in table) {
            var fieldName = pair.Key as string;
            if (string.IsNullOrEmpty(fieldName)) throw new Exception($"Const:{name} Field:{fieldName} 参数出错");
            var value = pair.Value;
            var field = new FieldConst() { Name = fieldName };
            switch (value.valueType) {
                case ScriptValue.longValueType:
                    field.Type = BasicEnum.INT64;
                    field.Value = value.longValue.ToString() + "L";
                    break;
                case ScriptValue.stringValueType:
                    field.Type = BasicEnum.STRING;
                    field.Value = "\"" + value.stringValue + "\"";
                    break;
                case ScriptValue.doubleValueType:
                case ScriptValue.objectValueType:
                    field.Type = BasicEnum.INT32;
                    field.Value = value.ToString();
                    break;
                default: throw new Exception("不支持此常量类型 " + value.ValueTypeName);
            }
            consts.Fields.Add(field);
        }
        Consts[name.Substring(CONST_KEYWORD.Length)] = consts;
    }
    void ParseClass(string name, ScriptMap table) {
        var classes = new PackageClass();
        foreach (var pair in table) {
            var fieldName = pair.Key as string;
            var val = pair.Value.ToString();
            if (string.IsNullOrEmpty(fieldName)) throw new Exception($"Class:{name} Field:{fieldName} 参数出错 参数模版 \"[索引],[类型],[是否数组=false],[注释]\"");
            var infos = val.Split(',');
            if (infos.Length < 2) throw new Exception($"Class:{name} Field:{fieldName} 参数出错 参数模版 \"[索引],[类型],[是否数组=false],[注释]\"");
            var packageField = new FieldClass(this) {
                Name = fieldName,
                Index = infos[0].ToInt32(),
                Type = infos[1],
                Array = infos.Length > 2 && infos[2].ToBoolean(),
                Comment = infos.Length > 3 ? infos[3] : "",
            };
            if (!packageField.IsBasic) {
                if (!Script.HasGlobal(packageField.Type) &&                             //判断网络协议自定义类
                    !Script.HasGlobal(ENUM_KEYWORD + packageField.Type) &&              //判断是否是枚举
                    !Script.HasGlobal(MESSAGE_KEYWORD + packageField.Type) &&           //判断网络协议自定义类
                    !Script.HasGlobal(TABLE_KEYWORD + packageField.Type)                //判断Table内嵌类
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
        TypeManager.PushAssembly(typeof(Scorpio.Commons.FileUtil).Assembly);
        TypeManager.PushAssembly(GetType().Assembly);
        Script.LoadLibraryV1();
        var global = Script.Global;
        var globalKeys = global.GetKeys();
        var files = Directory.Exists(dir) ? Directory.GetFiles(dir, "*.sco", SearchOption.AllDirectories) : (File.Exists(dir) ? new string[] { dir } : new string[0]);
        foreach (var file in files) { Script.LoadFile(file); }
        var keys = global.GetKeys();
        foreach (var name in keys) {
            if (globalKeys.Contains(name)) { continue; }
            var table = global.GetValue(name).Get<ScriptMap>();
            if (table == null) { continue; }
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
