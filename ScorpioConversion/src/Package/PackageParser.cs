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

    private Dictionary<string, List<PackageEnum>> mEnums = new Dictionary<string, List<PackageEnum>>();
    private Dictionary<string, List<PackageConst>> mConsts = new Dictionary<string, List<PackageConst>>();
    private Dictionary<string, List<PackageField>> mMessages = new Dictionary<string, List<PackageField>>();
    private Dictionary<string, List<PackageField>> mTables = new Dictionary<string, List<PackageField>>();
    private Dictionary<string, List<PackageField>> mClasses = new Dictionary<string, List<PackageField>>();
    private Script mScript = null;
    public Dictionary<string, List<PackageEnum>> Enums { get { return mEnums; } }
    public Dictionary<string, List<PackageConst>> Consts { get { return mConsts; } }
    public Dictionary<string, List<PackageField>> Messages { get { return mMessages; } }
    public Dictionary<string, List<PackageField>> Tables { get { return mTables; } }
    public Dictionary<string, List<PackageField>> Classes { get { return mClasses; } }
    void ParseEnum(string name, ScriptTable table) {
        var enums = new List<PackageEnum>();
        var itor = table.GetIterator();
        while (itor.MoveNext()) {
            var fieldName = itor.Current.Key as string;
            var val = itor.Current.Value as ScriptNumber;
            if (string.IsNullOrEmpty(fieldName) || val == null) throw new Exception(string.Format("Enum:{0} Field:{1} 参数出错", name, fieldName));
            enums.Add(new PackageEnum() {
                Index = Convert.ToInt32(val.ObjectValue),
                Name = fieldName,
            });
        }
        enums.Sort((m1, m2) => { return m1.Index.CompareTo(m2.Index); });
        mEnums[name.Substring(ENUM_KEYWORD.Length)] = enums;
    }
    void ParseConst(string name, ScriptTable table) {
        var consts = new List<PackageConst>();
        var itor = table.GetIterator();
        while (itor.MoveNext()) {
            var fieldName = itor.Current.Key as string;
            if (string.IsNullOrEmpty(fieldName)) throw new Exception(string.Format("Const:{0} Field:{1} 参数出错", name, fieldName));
            var pack = new PackageConst();
            pack.Name = fieldName;
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
            consts.Add(pack);
        }
        mConsts[name.Substring(CONST_KEYWORD.Length)] = consts;
    }
    void ParseClass(string name, ScriptTable table) {
        var fields = new List<PackageField>();
        var itor = table.GetIterator();
        while (itor.MoveNext()) {
            var fieldName = itor.Current.Key as string;
            var val = itor.Current.Value as ScriptString;
            if (string.IsNullOrEmpty(fieldName) || val == null) throw new Exception(string.Format("Class:{0} Field:{1} 参数出错 参数模版 \"索引,类型,是否数组=false,注释\"", name, fieldName));
            var infos = val.Value.Split(',');
            if (infos.Length < 2) throw new Exception(string.Format("Class:{0} Field:{1} 参数出错 参数模版 \"索引,类型,是否数组=false,注释\"", name, fieldName));
            var packageField = new PackageField() {
                Index = infos[0].ToInt32(),
                Type = infos[1],
                Name = fieldName,
                Array = infos.Length > 2 && infos[2].ToBoolean(),
                Comment = infos.Length > 3 ? infos[3] : "",
            };
            if (!packageField.IsBasic) {
                if (mScript.HasValue(ENUM_KEYWORD + packageField.Type)) {
                    packageField.Enum = true;
                } else if (mScript.HasValue(CONST_KEYWORD + packageField.Type)) {
                    packageField.Const = true;
                } else if (!mScript.HasValue(packageField.Type) &&                              //判断网络协议自定义类
                            !mScript.HasValue(MESSAGE_KEYWORD + packageField.Type) &&           //判断数据库内嵌类
                            !mScript.HasValue(TABLE_KEYWORD + packageField.Type)                //判断Table内嵌类
                           ) {
                    throw new Exception(string.Format("Class:{0} Field:{1} 未知类型:{2}", name, fieldName, packageField.Type));
                }
            }
            fields.Add(packageField);
        }
        fields.Sort((m1, m2) => { return m1.Index.CompareTo(m2.Index); });
        if (name.StartsWith(MESSAGE_KEYWORD)) {         //协议结构
            mMessages[name.Substring(MESSAGE_KEYWORD.Length)] = fields;
        } else if (name.StartsWith(TABLE_KEYWORD)) {    //table结构
            mTables[name.Substring(TABLE_KEYWORD.Length)] = fields;
        } else {
            mClasses[name] = fields;
        }
    }
    public void Parse(string dir, bool clear = false) {
        if (clear) {
            mEnums.Clear();
            mConsts.Clear();
            mMessages.Clear();
            mTables.Clear();
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
