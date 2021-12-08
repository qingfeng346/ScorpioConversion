using System;
using Scorpio;
public class ScriptGenerator : IGenerator {
    public ScriptValue Value { get; private set; }
    public ScriptGenerator(string language, ScriptValue value) : base(language) {
        this.Value = value;
    }
    public bool __Call(string functionName, out ScriptValue ret, params object[] args) {
        var func = Value.GetValue(functionName);
        if (func.valueType == ScriptValue.scriptValueType) {
            try {
                ret = func.call(Value, args);
                return true;
            } catch (Exception e) {
                throw new Exception($"ScriptInstance Call is error Type:ScriptGenerator  Function:{functionName} error:{e}");
            }
        }
        ret = ScriptValue.Null;
        return false;
    }
    public override string GetDataPath(LanguageInfo languageInfo, string name) {
        if (__Call("GetDataPath", out var ret, languageInfo, name)) {
            return ret.ToString();
        }
        return base.GetDataPath(languageInfo, name);
    }
    public override string GetCodePath(LanguageInfo languageInfo, string name) {
        if (__Call("GetCodePath", out var ret, languageInfo, name)) {
            return ret.ToString();
        }
        return base.GetDataPath(languageInfo, name);
    }
    public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass) {
        __Call("GenerateTableClass", out var ret, packageName, tableClassName, dataClassName, fileMD5, packageClass);
        return ret.ToString();
    }
    public override string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID = false) {
        __Call("GenerateDataClass", out var ret, packageName, className, packageClass, createID);
        return ret.ToString();
    }
    public override string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum) {
        __Call("GenerateEnumClass", out var ret, packageName, className, packageEnum);
        return ret.ToString();
    }
}
