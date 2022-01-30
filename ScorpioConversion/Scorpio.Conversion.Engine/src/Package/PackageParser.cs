using System.Collections.Generic;
using System.IO;
using System.Text;
using Scorpio.Userdata;
namespace Scorpio.Conversion.Engine {
    public interface IPackage {

    }
    public class PackageParser {
        private const string ENUM_KEYWORD = "enum_";            //枚举类型关键字
        private const string CONST_KEYWORD = "const_";          //常量类型关键字
        private const string TABLE_KEYWORD = "table_";          //Table格式关键字

        public SortedDictionary<string, PackageEnum> Enums { get; set; } = new SortedDictionary<string, PackageEnum>();
        public SortedDictionary<string, PackageConst> Consts { get; set; } = new SortedDictionary<string, PackageConst>();
        public SortedDictionary<string, PackageClass> Classes { get; set; } = new SortedDictionary<string, PackageClass>();
        public Script Script { get; private set; } = new Script();
        void ParseEnum(string name, ScriptMap table) {
            var enums = new PackageEnum();
            foreach (var pair in table) {
                var fieldName = pair.Key as string;
                if (string.IsNullOrEmpty(fieldName)) throw new System.Exception($"Enum:{name} Field:{fieldName} 参数出错");
                enums.Fields.Add(new EnumField() {
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
                if (string.IsNullOrEmpty(fieldName)) throw new System.Exception($"Const:{name} Field:{fieldName} 参数出错");
                var value = pair.Value;
                var field = new ConstField() { Name = fieldName };
                switch (value.valueType) {
                    case ScriptValue.longValueType:
                        field.Type = BasicEnum.INT64;
                        field.Value = value.ToString();
                        break;
                    case ScriptValue.stringValueType:
                        field.Type = BasicEnum.STRING;
                        field.Value = value.ToString();
                        break;
                    case ScriptValue.doubleValueType:
                    case ScriptValue.objectValueType:
                        field.Type = BasicEnum.INT32;
                        field.Value = value.ToInt32().ToString();
                        break;
                    default: throw new System.Exception($"不支持此常量类型 : {value.ValueTypeName}");
                }
                consts.Fields.Add(field);
            }
            name = name.Substring(CONST_KEYWORD.Length);
            consts.Name = name;
            Consts[name] = consts;
        }
        void ParseClass(string name, ScriptArray array) {
            var classes = new PackageClass();
            foreach (var field in array) {
                var fieldName = field.GetValue("name");
                var fieldType = field.GetValue("type");
                if (!fieldName.IsString || !fieldType.IsString) {
                    throw new System.Exception($"Class:{name} Field error, 字段只支持 name(字段名) type(字段类型) array(是否是数组) def(默认值) comment(注释) 属性");
                }
                var fieldDefault = field.GetValue("def");
                var fieldComment = field.GetValue("comment");
                var packageField = new ClassField(this) {
                    Name = fieldName.ToString(),
                    Type = fieldType.ToString(),
                    IsArray = field.GetValue("array").IsTrue,
                    Default = fieldDefault.IsString ? fieldDefault.stringValue : null,
                    Comment = fieldComment.IsString ? fieldComment.stringValue : null,
                };
                if (!packageField.IsBasic) {
                    if (!Script.HasGlobal(ENUM_KEYWORD + packageField.Type) &&              //判断是否是枚举
                        !Script.HasGlobal(TABLE_KEYWORD + packageField.Type)                //判断Table内嵌类
                        ) {
                        throw new System.Exception($"Class:{name} Field:{packageField.Name} 未知类型:{packageField.Type}");
                    }
                }
                classes.Fields.Add(packageField);
            }
            if (name.StartsWith(TABLE_KEYWORD)) {    //table结构
                name = name.Substring(TABLE_KEYWORD.Length);
                Classes[name] = classes;
            }
            classes.Name = name;
        }
        public int GetEnumValue(string name, string value) {
            var ret = Enums[name].Fields.Find((field) => field.Name == value);
            if (ret == null) throw new System.Exception($"枚举:{name} 找不到枚举值:{value}");
            return ret.Index;
        }
        public bool IsEnum(string name) {
            return Enums.ContainsKey(name);
        }
        public PackageEnum GetEnum(string name) {
            if (Enums.TryGetValue(name, out var value)) {
                return value;
            }
            return null;
        }
        public bool IsClass(string name) {
            return Classes.ContainsKey(name);
        }
        public PackageClass GetClass(string name) {
            if (Classes.TryGetValue(name, out var value)) {
                return value;
            }
            throw new System.Exception($"找不到自定义类 : {name}");
        }
        public void Parse(string dir, bool clear = false) {
            if (clear) {
                Enums.Clear();
                Consts.Clear();
                Classes.Clear();
            }
            TypeManager.PushAssembly(typeof(Scorpio.Commons.FileUtil).Assembly);
            TypeManager.PushAssembly(GetType().Assembly);
            Script.LoadLibraryV1();
            var global = Script.Global;
            var globalKeys = new HashSet<string>(global.GetKeys());
            var files = Directory.Exists(dir) ? Directory.GetFiles(dir, "*.sco", SearchOption.AllDirectories) : (File.Exists(dir) ? new string[] { dir } : System.Array.Empty<string>());
            foreach (var file in files) { Script.LoadFile(file); }
            var keys = global.GetKeys();
            foreach (var name in keys) {
                if (globalKeys.Contains(name)) { continue; }
                var value = global.GetValue(name);
                var table = value.Get<ScriptMap>();
                if (table != null) {
                    if (name.StartsWith(ENUM_KEYWORD)) {                //枚举类型
                        ParseEnum(name, table);
                    } else if (name.StartsWith(CONST_KEYWORD)) {        //常量类型
                        ParseConst(name, table);
                    }
                    continue;
                }
                var array = value.Get<ScriptArray>();
                if (array != null) {
                    ParseClass(name, array);
                }
            }
        }
    }
}