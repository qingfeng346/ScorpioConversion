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
    public override string GenerateDataClass(string packageName, string tableClassName, string dataClassName, string suffix, string fileMD5, PackageClass packageClass) {
        return __Call("GenerateDataClass", packageClass, tableClassName, dataClassName, suffix, fileMD5, packageClass).ToString();
    }

    public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string suffix, string fileMD5, PackageClass packageClass) {
        return __Call("GenerateTableClass", packageClass, tableClassName, dataClassName, suffix, fileMD5, packageClass).ToString();
    }
}
