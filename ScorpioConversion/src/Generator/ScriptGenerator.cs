using System;
using Scorpio;
public class ScriptGenerator : IGenerator {
    public ScriptValue Value { get; private set; }
    public ScriptGenerator(string language, ScriptValue value) : base(language) {
        this.Value = value;
    }
    public ScriptValue __Call(string functionName, params object[] args) {
        var func = Value.GetValue(functionName);
        if (func.valueType == ScriptValue.scriptValueType) {
            try {
                return func.call(Value, args);
            } catch (Exception e) {
                throw new Exception($"ScriptInstance Call is error Type:ScriptGenerator  Function:{functionName} error:{e}");
            }
        }
        return ScriptValue.Null;
    }
    public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass) {
        return __Call("GenerateTableClass", packageName, tableClassName, dataClassName, fileMD5, packageClass).ToString();
    }
    public override string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID = false) {
        return __Call("GenerateDataClass", packageName, className, packageClass, createID).ToString();
    }
    public override string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum) {
        return __Call("GenerateEnumClass", packageName, className, packageEnum).ToString();
    }
}
